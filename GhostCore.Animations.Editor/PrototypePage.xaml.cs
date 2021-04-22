using GhostCore.Animations.Core;
using GhostCore.Animations.Data;
using GhostCore.Animations.Data.Layers;
using GhostCore.Animations.Rendering;
using GhostCore.Graphics.Colors;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Svg;
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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace GhostCore.Animations.Editor
{
    public sealed partial class PrototypePage : Page
    {
        private PlaybackController _playbackController;

        public PrototypePage()
        {
            InitializeComponent();
            Loaded += PrototypePage_Loaded;
        }

        private async void PrototypePage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= PrototypePage_Loaded;
            Unloaded += PrototypePage_Unloaded;

            RuntimeBitmapCache.BitmapLoadRequested += RuntimeBitmapCache_BitmapLoadRequested;
            RuntimeSvgCache.SvgLoadRequested += RuntimeSvgCache_SvgLoadRequested;

            var scene = CreateTestScene();

            sceneRenderer.Scenes.Add(scene);

            await sceneRenderer.Initialize();

            var timeline = new Timeline()
            {
                Duration = 3,
                WrapMode = PlayableWrapMode.Loop
            };

            _playbackController = new PlaybackController(scene, timeline);
            _playbackController.FrameDispatched += PlaybackController_FrameDispatched;
        }

        private void PlaybackController_FrameDispatched(object sender, float e)
        {
            sceneRenderer.SetCurrentTime(e);
        }

        private void PrototypePage_Unloaded(object sender, RoutedEventArgs e)
        {
            RuntimeBitmapCache.BitmapLoadRequested -= RuntimeBitmapCache_BitmapLoadRequested;
            RuntimeSvgCache.SvgLoadRequested -= RuntimeSvgCache_SvgLoadRequested;
            Unloaded -= PrototypePage_Unloaded;
        }

        private async Task RuntimeSvgCache_SvgLoadRequested(SvgLoadEventArgs args)
        {
            if (RuntimeSvgCache.DeviceCache[args.ResourceCreator].ContainsKey(args.Source))
            {
                args.SvgDocument = RuntimeSvgCache.DeviceCache[args.ResourceCreator][args.Source];
                return;
            }

            if (args.Source.Contains("ms-appx"))
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(args.Source));
                var stream = await file.OpenReadAsync();

                var svg = await CanvasSvgDocument.LoadAsync(args.ResourceCreator, stream);
                args.SvgDocument = svg;
            }
        }

        private async Task RuntimeBitmapCache_BitmapLoadRequested(BitmapLoadEventArgs args)
        {
            if (RuntimeBitmapCache.DeviceCache[args.ResourceCreator].ContainsKey(args.Source))
            {
                args.Bitmap = RuntimeBitmapCache.DeviceCache[args.ResourceCreator][args.Source];
                return;
            }

            if (args.Source.Contains("ms-appx"))
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(args.Source));
                var stream = await file.OpenReadAsync();

                var bitmap = await CanvasBitmap.LoadAsync(args.ResourceCreator, stream);
                args.Bitmap = bitmap;
            }
        }


        private Scene CreateTestScene()
        {
            var center = new Vector2(0.5f, 0.5f);
            var layers = new List<ILayer>
                {
                    new ImageLayer()
                    {
                        Name = "bg",
                        Source = "ms-appx:///Assets/Map/bg.jpg",
                        Type = ImageType.Bitmap
                    }
                };
            var scene = new Scene()
            {
                BackdropColor = new RGBA(0.5f, 0.5f, 0.5f, 1),
                Name = "Test Scene",
                RenderInfo = new RenderInfo
                {
                    FrameRate = 60.0f,
                    RenderScale = 1,
                    RenderSize = new Vector2(960, 540)
                },
                Layers = layers
            };

            var ugIcon = new ImageLayer()
            {
                Name = "undergroundIcon",
                Transform = new TransformData(center, Vector2.Zero, Vector2.One, 0),
                Source = "ms-appx:///Assets/Map/ugIcon.png",
                Type = ImageType.Bitmap,
                Animations = new List<AnimationCurve>()
                {
                    new AnimationCurve()
                    {
                        IsAnimationEnabled = true,
                        TargetProperty = "Transform.Scale",
                        WrapMode = PlayableWrapMode.PlayOnce,
                        Keyframes = new Keyframe[]
                        {
                            new Keyframe { Time = 0, Value = 0 },
                            new Keyframe { Time = 0.266f, Value = 1.25f },
                            new Keyframe { Time = 0.433f, Value = 0.946f },
                            new Keyframe { Time = 0.632f, Value = 1.123f },
                            new Keyframe { Time = 0.832f, Value = 0.96f },
                            new Keyframe { Time = 1, Value = 1 },
                        }
                    }
                }
            };

            var dlrIcon = new ImageLayer()
            {
                Name = "dlrIcon",
                Transform = new TransformData(center, new Vector2(52, 0), Vector2.One, 0),
                Source = "ms-appx:///Assets/Map/dlrIcon.png",
                Type = ImageType.Bitmap,
                StartTime = 0.15f,
                Animations = new List<AnimationCurve>()
                {
                    new AnimationCurve()
                    {
                        IsAnimationEnabled = true,
                        TargetProperty = "Transform.Scale",
                        WrapMode = PlayableWrapMode.PlayOnce,
                        Keyframes = new Keyframe[]
                        {
                            new Keyframe { Time = 0, Value = 0 },
                            new Keyframe { Time = 0.266f, Value = 1.25f },
                            new Keyframe { Time = 0.433f, Value = 0.946f },
                            new Keyframe { Time = 0.632f, Value = 1.123f },
                            new Keyframe { Time = 0.832f, Value = 0.96f },
                            new Keyframe { Time = 1, Value = 1 },
                        }
                    }
                }
            };

            var bankText = new TextLayer()
            {
                Name = "bankText",
                Transform = new TransformData(Vector2.Zero, new Vector2(0, 60), Vector2.One, 0),
                Text = "Bank",
                Color = new RGBA(0, 0, 0, 1),
                FontSize = 25,
                //UseRectangleMask = true,
                //RectangleMask = new Rect(0, 54, 100, 33),
                Animations = new List<AnimationCurve>()
                {
                    new AnimationCurve()
                    {
                        IsAnimationEnabled = true,
                        WrapMode = PlayableWrapMode.PlayOnce,
                        TargetProperty = "Transform.Position.Y",
                        Keyframes = new Keyframe[]
                        {
                            new Keyframe { Time = 0, Value = 100 },
                            new Keyframe { Time = 1f, Value = 54 },
                        }
                    }
                }
            };

            var btrem = new TextLayer()
            {
                Name = "bankTimeText",
                Transform = new TransformData(Vector2.Zero, new Vector2(0, 87), Vector2.One, 0),
                Text = "11",
                FontSize = 20,
                Color = new RGBA(0, 0, 0, 1),
                //IsMasked = true,
                //UseRectangleMask = true,
                //RectangleMask = new Rect(0, 87, 100, 33),
                Animations = new List<AnimationCurve>()
                {
                    new AnimationCurve()
                    {
                        IsAnimationEnabled = true,
                        WrapMode = PlayableWrapMode.PlayOnce,
                        TargetProperty = "Transform.Position.Y",
                        Keyframes = new Keyframe[]
                        {
                            new Keyframe { Time = 0, Value = 50 },
                            new Keyframe { Time = 1f, Value = 87 },
                        }
                    }
                },
                TextKeyframes = new List<TextKeyframe>()
                {
                    new TextKeyframe { Time = 0, Value = "1 min" },
                    new TextKeyframe { Time = 1f, Value = "2 min" },
                    new TextKeyframe { Time = 2f, Value = "3 min" },
                    new TextKeyframe { Time = 3f, Value = "4 min" },
                    new TextKeyframe { Time = 4f, Value = "5 min" },
                }
            };

            var compositeLayer = new CompositeLayer()
            {
                Name = "bankLayer",
                Transform = new TransformData(Vector2.Zero, new Vector2(250, 100), Vector2.One, 0),
                Children = new List<ILayer>()
                {
                    ugIcon,
                    dlrIcon,
                    bankText,
                    btrem,
                },
                Opacity = 0,
                Animations = new List<AnimationCurve>()
                {
                    new AnimationCurve()
                    {
                        IsAnimationEnabled = true,
                        TargetProperty = "Opacity",
                        WrapMode = PlayableWrapMode.PlayOnce,
                        Keyframes = new Keyframe[]
                        {
                            new Keyframe { Time = 0, Value = 0 },
                            new Keyframe { Time = 0.5f, Value = 1 },
                        }
                    }
                }
            };

            layers.Add(compositeLayer);

            return scene;
        }


        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            _playbackController.Seek(0);
            _playbackController.Play();
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            _playbackController.Pause();
            _playbackController.Seek((float)e.NewValue);
        }
    }
}
