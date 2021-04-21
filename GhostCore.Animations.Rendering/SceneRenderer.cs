using GhostCore.Animations.Core;
using GhostCore.Animations.Data.Layers;
using GhostCore.Animations.Rendering;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Svg;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
namespace GhostCore.Animations.Rendering
{
    public sealed partial class SceneRenderer : ContentControl
    {
        #region Events

        public event AsyncTypedEventHandler<SceneRenderer, BitmapLoadEventArgs> BitmapLoadRequested;
        public event AsyncTypedEventHandler<SceneRenderer, SvgLoadEventArgs> SvgLoadRequested;

        #endregion

        #region Fields

        private float _curTime;
        private bool _resCreated;
        private CanvasControl _canvas;
        private List<LayerRendererBase> _renderers = new List<LayerRendererBase>();
        private Dictionary<string, IScene> _cachedComps = new Dictionary<string, IScene>();

        #endregion

        #region Props

        public List<IScene> Scenes
        {
            get { return (List<IScene>)GetValue(ScenesProperty); }
            set { SetValue(ScenesProperty, value); }
        }

        public static readonly DependencyProperty ScenesProperty =
            DependencyProperty.Register("Scenes", typeof(List<IScene>), typeof(SceneRenderer), new PropertyMetadata(new List<IScene>()));

        #endregion

        #region Init

        public SceneRenderer()
        {
            Loaded += FoundationCompositionRenderer_Loaded;
            Unloaded += FoundationCompositionRenderer_Unloaded;
        }

        private void FoundationCompositionRenderer_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= FoundationCompositionRenderer_Unloaded;
            if (RuntimeBitmapCache.DeviceCache.TryGetValue(_canvas, out var bc))
            {
                bc.Dispose();
                bc.Clear();
                RuntimeBitmapCache.DeviceCache.Remove(_canvas);
            }

            if (RuntimeSvgCache.DeviceCache.TryGetValue(_canvas, out var sc))
            {
                sc.Dispose();
                sc.Clear();
                RuntimeSvgCache.DeviceCache.Remove(_canvas);
            }
        }

        private async void FoundationCompositionRenderer_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= FoundationCompositionRenderer_Loaded;
            await Initialize();
        }

        public async Task Initialize()
        {
            _cachedComps = new Dictionary<string, IScene>();

            int i = 0;
            foreach (var scene in Scenes)
            {
                foreach (var layer in scene.Layers)
                {
                    if (layer is ISimpleAsyncInitializable refLayer)
                        await refLayer.InitializeAsync();

                    var renderer = LayerRendererFactory.CreateRenderer(layer);
                    renderer.ParentScene = scene;
                    renderer.ParentSceneIndex = i++;
                    _renderers.Add(renderer);
                }
                _cachedComps.Add(scene.Name ?? Guid.NewGuid().ToString(), scene);
            }

            _canvas = new CanvasControl();
            _canvas.CreateResources += Canvas_CreateResources;
            _canvas.Draw += Canvas_Draw;

            Content = _canvas;
        }

        #endregion

        #region Resource Creation

        private void Canvas_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        private async Task CreateResourcesAsync(CanvasControl sender)
        {
            var rdlayers = new List<IResourceDependentLayer>();

            foreach (var scene in Scenes)
            {
                var rdl = scene.ExtractResourceDependentLayers();
                rdlayers.AddRange(rdl);
            }


            foreach (var img in rdlayers)
            {
                if (img.Type == ImageType.Bitmap)
                {
                    if (!RuntimeBitmapCache.DeviceCache.TryGetValue(_canvas, out var cache))
                    {
                        cache = new BitmapResourceCache();
                        RuntimeBitmapCache.DeviceCache.Add(_canvas, cache);
                    }

                    if (!cache.ContainsKey(img.Source))
                    {
                        var args = new BitmapLoadEventArgs
                        {
                            Source = img.Source,
                            ResourceCreator = _canvas
                        };

                        await BitmapLoadRequested?.Invoke(this, args);

                        if (args.Bitmap != null)
                        {
                            cache.Add(img.Source, args.Bitmap);
                        }
                    }
                }

                if (img.Type == ImageType.Svg)
                {
                    if (!RuntimeSvgCache.DeviceCache.TryGetValue(_canvas, out var scache))
                    {
                        scache = new SvgResourceCache();
                        RuntimeSvgCache.DeviceCache.Add(_canvas, scache);
                    }

                    if (!scache.ContainsKey(img.Source))
                    {
                        var args = new SvgLoadEventArgs
                        {
                            Source = img.Source,
                            ResourceCreator = _canvas
                        };

                        await SvgLoadRequested?.Invoke(this, args);

                        if (args.SvgDocument != null)
                            scache.Add(img.Source, args.SvgDocument);
                    }
                }
            }

            foreach (var rnd in _renderers)
            {
                await rnd.Initialze(sender);
            }

            _resCreated = true;
        }


        #endregion

        #region API

        public void SetCurrentTime(float time)
        {
            _curTime = time;
            foreach (var rnd in _renderers)
            {
                rnd.CurrentTime = _curTime;
                rnd.UpdateAnimationState(_curTime);
            }

            _canvas.Invalidate();
        }

        public void SetCurrentTimeForComp(string compName, float time)
        {
            var comp = _cachedComps[compName];
            SetCurrentTimeForComp(comp, time);
        }

        public void SetCurrentTimeForComp(FoundationComposition comp, float time)
        {
            _curTime = time;

            Parallel.ForEach(_renderers.Where(x => x.ParentComposition == comp), rnd =>
            {
                rnd.CurrentTime = _curTime;
                rnd.UpdateAnimationState(_curTime);
            });

            _canvas.Invalidate();
        }

        public void SetCurrentTimeForComp(int compIndex, float time)
        {
            _curTime = time;

            Parallel.ForEach(_renderers.Where(x => x.ParentSceneIndex == compIndex), rnd =>
            {
                rnd.CurrentTime = _curTime;
                rnd.UpdateAnimationState(_curTime);
            });

            _canvas.Invalidate();
        }

        #endregion

        #region Rendering

        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (!_resCreated)
                return;

            var ds = args.DrawingSession;

            if (!UseMultipleCompositions)
                ds.Clear(Composition.BackgroundColor);
            else
                ds.Clear(Colors.Transparent);


            foreach (var renderer in _renderers)
            {
                renderer.Render(ds);
            }
        }

        #endregion
    }

}
