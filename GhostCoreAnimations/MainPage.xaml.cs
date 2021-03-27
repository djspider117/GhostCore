using GhostCore.Animations;
using GhostCore.Animations.Controls;
using GhostCore.Animations.Layers;
using GhostCore.Animations.Rendering;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Svg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GhostCoreAnimations
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AnimationPlaybackController _animController;

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            renderer.BitmapLoadRequested += Renderer_BitmapLoadRequested;
            renderer.SvgLoadRequested += Renderer_SvgLoadRequested;
        }

        private async Task Renderer_SvgLoadRequested(FoundationCompositionRenderer sender, SvgLoadEventArgs args)
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

        private async Task Renderer_BitmapLoadRequested(FoundationCompositionRenderer sender, BitmapLoadEventArgs args)
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

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            _animController.Dispose();
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await FoundationCompositionReader.GetDefault().ReadFile("ms-appx:///Assets/comp.xml");

            //FoundationComposition comp = await CreateComp1(new TransformData(new Vector2(0.5f), new Vector2(1931, 702), new Vector2(1), 0));
            //FoundationComposition comp2 = await CreateComp3(new TransformData(new Vector2(0.5f), new Vector2(2231, 702), new Vector2(1), 0));
            //FoundationComposition comp3 = await CreateComp3(new TransformData(new Vector2(0.5f), new Vector2(1231, 702), new Vector2(1), 0));
            //FoundationComposition comp4 = await CreateComp3(new TransformData(new Vector2(0.5f), new Vector2(1831, 702), new Vector2(1), 0));
            //FoundationComposition comp5 = await CreateComp3(new TransformData(new Vector2(0.5f), new Vector2(631, 702), new Vector2(1), 0));

            var comp = await CreateTestComp2();

            //XmlSerializer xml = new XmlSerializer(typeof(FoundationComposition));


            //var stream = new MemoryStream();

            //TextWriter tw = new StreamWriter(stream);

            //xml.Serialize(tw, comp);

            //FileSavePicker fileSavePicker = new FileSavePicker();
            //fileSavePicker.FileTypeChoices.Add("Xml document", new List<string>() { ".xml" });

            //var file = await fileSavePicker.PickSaveFileAsync();

            //await FileIO.WriteBytesAsync(file, stream.ToArray());

            renderer.UseMultipleCompositions = true;
            renderer.Compositions.Add(comp);
            //renderer.Compositions.Add(comp2);
            //renderer.Compositions.Add(comp3);
            //renderer.Compositions.Add(comp4);
            //renderer.Compositions.Add(comp5);

            await renderer.Initialize();

            //renderer2.Composition = comp2;
            //await renderer2.Initialize();

            //renderer3.Composition = comp3;
            //await renderer3.Initialize();

            //renderer4.Composition = comp4;
            //await renderer4.Initialize();

            //renderer5.Composition = comp5;
            //await renderer5.Initialize();

            _animController = new AnimationPlaybackController(comp);
            _animController.FrameDispatched += AnimController_FrameDispatched;
        }

        private async static Task<FoundationComposition> CreateTestComp2()
        {
            var center = new Vector2(0.5f, 0.5f);

            var layers = new List<ILayer>()
            {
                new ImageLayer()
                {
                    Name = "bg",
                    Source = "ms-appx:///Assets/Map/bg.jpg",
                    Type = ImageType.Bitmap
                }
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
                        WrapMode = AnimationWrapMode.PlayOnce,
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
                        WrapMode = AnimationWrapMode.PlayOnce,
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
                Color = Color.FromArgb(255, 0, 0, 0),
                IsMasked = true,
                FontSize = 25,
                UseRectangleMask = true,
                RectangleMask = new Rect(0, 54, 100, 33),
                Animations = new List<AnimationCurve>()
                {
                    new AnimationCurve()
                    {
                        IsAnimationEnabled = true,
                        WrapMode = AnimationWrapMode.PlayOnce,
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
                Color = Color.FromArgb(255, 0, 0, 0),
                IsMasked = true,
                UseRectangleMask = true,
                RectangleMask = new Rect(0, 87, 100, 33),
                Animations = new List<AnimationCurve>()
                {
                    new AnimationCurve()
                    {
                        IsAnimationEnabled = true,
                        WrapMode = AnimationWrapMode.PlayOnce,
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
                //Transform = new TransformData(Vector2.Zero, new Vector2(1686, 1186), Vector2.One, 0),
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
                        WrapMode = AnimationWrapMode.PlayOnce,
                        Keyframes = new Keyframe[]
                        {
                            new Keyframe { Time = 0, Value = 0 },
                            new Keyframe { Time = 0.5f, Value = 1 },
                        }
                    }
                }
            };

            var compositeLayer2 = new CompositeLayer()
            {
                Name = "bankLayer",
                //Transform = new TransformData(Vector2.Zero, new Vector2(1686, 1186), Vector2.One, 0),
                Transform = new TransformData(Vector2.Zero, new Vector2(550, 400), Vector2.One, 0),
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
                        WrapMode = AnimationWrapMode.PlayOnce,
                        Keyframes = new Keyframe[]
                        {
                            new Keyframe { Time = 0, Value = 0 },
                            new Keyframe { Time = 0.5f, Value = 1 },
                        }
                    }
                }
            };
            layers.Add(compositeLayer);
            layers.Add(new PathLayer()
            {
                //Data = "F0 M 656.500,400.500 C 656.500,350.637 598.572,307.493 514.292,286.708 C 493.507,202.428 450.363,144.500 400.500,144.500 C 350.637,144.500 307.493,202.428 286.708,286.708 C 202.428,307.493 144.500,350.637 144.500,400.500 C 144.500,450.363 202.428,493.507 286.708,514.292 C 307.493,598.572 350.637,656.500 400.500,656.500 C 450.363,656.500 493.507,598.572 514.292,514.292 C 598.572,493.507 656.500,450.363 656.500,400.500 ZM 581.519,219.481 C 546.261,184.222 474.793,194.676 400.500,239.574 C 326.207,194.676 254.739,184.222 219.481,219.481 C 184.222,254.739 194.676,326.207 239.574,400.500 C 194.676,474.792 184.222,546.261 219.481,581.519 C 254.739,616.778 326.207,606.324 400.500,561.426 C 474.793,606.324 546.261,616.778 581.519,581.519 C 616.778,546.261 606.324,474.792 561.426,400.500 C 606.324,326.207 616.778,254.739 581.519,219.481 ZM 688.500,688.500 L 112.500,688.500 L 112.500,112.500 L 688.500,112.500 L 688.500,688.500 Z",
                //Data = "M 100 350 q 150 -300 300 0",
                Data = "M3.8,958.9c101.6-0.9,232.4-0.3,374.1,5.2c78.2,3,150.6,7.2,226.6,21.2c59.7,10.9,105.4,21.1,105.4,21.1c47.6,10.5,59.5,14.6,94.9,21.1c31.7,5.8,60.8,11.1,98.9,13c45.6,2.3,91.7,0.1,136.2-10.4c45.7-10.8,83.1-31.9,122.3-56.8c82.5-52.4,171.9-96.7,267.1-120.1c11.3-2.8,22.7-5.3,34.2-7.4c49.4-9.3,86.2-11,125.6-12.7c69-3.1,105-4.7,153.7,4.8c53.3,10.5,97.8,29.7,108,34.3c36.2,16,53.9,28.8,98.1,42.6c31.5,9.8,54.5,13.3,60,14c17.2,2.5,42.1,4.7,88.3,1.3c65.3-4.7,127.2-16.8,189-38.5c77.1-27,152.3-50.7,232.5-65.6c76.6-14.2,153.6-26.3,230.7-37.8c74.4-11.1,151.9-16.2,223.9-38.6c108.9-33.9,184.2-85.7,239.8-123.8c101.9-70,203.1-160.9,276.7-260.8c2.3-3.2,9.9-14.8,21.1-30.3c32.4-45.1,68.4-88,110.5-124.4c43.4-37.5,91.6-61.4,142.5-86.4c25.9-12.7,114-54.9,241.1-89.6c55-15,101.9-24.4,135.7-30.3",
                DashStyle = CanvasDashStyle.Dot,
                Transform = new TransformData(new Vector2(0.5f), new Vector2(1931, 702), new Vector2(1), 0),
                DashOffset = 15f,
                DashLen = 10f,
                TrimEnd = 0,
                Color = Colors.Black,
                Animations = new List<AnimationCurve>()
                        {
                            new AnimationCurve()
                            {
                                IsAnimationEnabled = true,
                                WrapMode = AnimationWrapMode.PlayOnce,
                                TargetProperty = "TrimEnd",
                                Keyframes = new Keyframe[]
                                {
                                    new Keyframe() { Time = 0, Value = 0 },
                                    new Keyframe() { Time = 10f, Value = 1 },
                                }
                            },
                        }
            });

            var comp = new FoundationComposition
            {
                FrameRate = 60,
                BackgroundColor = Color.FromArgb(255, 125, 125, 125),
                Layers = layers
            };


            return comp;
        }


        private static FoundationComposition CreateTestComp1()
        {
            var comp = new FoundationComposition
            {
                FrameRate = 60,
                RenderScale = 1,
                RenderSize = new Size(1000, 900),
                BackgroundColor = Color.FromArgb(255, 125, 125, 125),
                Layers = new List<ILayer>
                {
                    new PathLayer()
                    {
                        Data = "F0 M 656.500,400.500 C 656.500,350.637 598.572,307.493 514.292,286.708 C 493.507,202.428 450.363,144.500 400.500,144.500 C 350.637,144.500 307.493,202.428 286.708,286.708 C 202.428,307.493 144.500,350.637 144.500,400.500 C 144.500,450.363 202.428,493.507 286.708,514.292 C 307.493,598.572 350.637,656.500 400.500,656.500 C 450.363,656.500 493.507,598.572 514.292,514.292 C 598.572,493.507 656.500,450.363 656.500,400.500 ZM 581.519,219.481 C 546.261,184.222 474.793,194.676 400.500,239.574 C 326.207,194.676 254.739,184.222 219.481,219.481 C 184.222,254.739 194.676,326.207 239.574,400.500 C 194.676,474.792 184.222,546.261 219.481,581.519 C 254.739,616.778 326.207,606.324 400.500,561.426 C 474.793,606.324 546.261,616.778 581.519,581.519 C 616.778,546.261 606.324,474.792 561.426,400.500 C 606.324,326.207 616.778,254.739 581.519,219.481 ZM 688.500,688.500 L 112.500,688.500 L 112.500,112.500 L 688.500,112.500 L 688.500,688.500 Z",
                        //Data = "M 100 350 q 150 -300 300 0",
                        DashStyle = CanvasDashStyle.Dash,
                        DashOffset = 5f,
                        DashLen = 10f,
                        TrimEnd = 0,
                        Color = Colors.Yellow,
                        Animations = new List<AnimationCurve>()
                        {
                            new AnimationCurve()
                            {
                                IsAnimationEnabled = true,
                                WrapMode = AnimationWrapMode.PingPong,
                                TargetProperty = "TrimEnd",
                                Keyframes = new Keyframe[]
                                {
                                    new Keyframe() { Time = 0, Value = 0 },
                                    new Keyframe() { Time = 10f, Value = 1 },
                                }
                            },
                            //new AnimationCurve()
                            //{
                            //    IsAnimationEnabled = true,
                            //    WrapMode = AnimationWrapMode.PingPong,
                            //    TargetProperty = "TrimBegin",
                            //    Keyframes = new Keyframe[]
                            //    {
                            //        new Keyframe() { Time = 0, Value = 0 },
                            //        new Keyframe() { Time = 10f, Value = 0.5f },
                            //    }
                            //},
                            new AnimationCurve()
                            {
                                IsAnimationEnabled = true,
                                WrapMode = AnimationWrapMode.PingPong,
                                TargetProperty = "DashOffset",
                                Keyframes = new Keyframe[]
                                {
                                    new Keyframe() { Time = 0, Value = 5 },
                                    new Keyframe() { Time = 10f, Value = 22f },
                                }
                            },
                            //new AnimationCurve()
                            //{
                            //    IsAnimationEnabled = true,
                            //    WrapMode = AnimationWrapMode.PingPong,
                            //    TargetProperty = "DashLen",
                            //    Keyframes = new Keyframe[]
                            //    {
                            //        new Keyframe() { Time = 0, Value = 10 },
                            //        new Keyframe() { Time = 5, Value = 25 },
                            //    }
                            //}
                        }
                    }
                    //    new ImageLayer()
                    //    {
                    //        Name = "svgTest",
                    //        Source = "ms-appx:///Assets/AJ_Digital_Camera.svg",
                    //        Transform = new TransformData(new Vector2(), new Vector2(450, 12), new Vector2(1, 1), 0),
                    //        SvgViewportHeight = 500,
                    //        SvgViewportWidth = 500,
                    //        Type = ImageType.Svg,
                    //        Animations = new List<AnimationCurve>
                    //        {
                    //            new AnimationCurve()
                    //            {
                    //                IsAnimationEnabled = true,
                    //                WrapMode = AnimationWrapMode.PingPong,
                    //                TargetProperty = "Transform.Scale.X",
                    //                Keyframes = new Keyframe[]
                    //                {
                    //                    new Keyframe() { Time = 0, Value = 1 },
                    //                    new Keyframe() { Time = 2.5f, Value = 0.5f },
                    //                }
                    //            },
                    //            new AnimationCurve()
                    //            {
                    //                IsAnimationEnabled = true,
                    //                WrapMode = AnimationWrapMode.PingPong,
                    //                TargetProperty = "Transform.Scale.Y",
                    //                Keyframes = new Keyframe[]
                    //                {
                    //                    new Keyframe() { Time = 0, Value = 1 },
                    //                    new Keyframe() { Time = 2.5f, Value = 0.5f },
                    //                }
                    //            },
                    //            new AnimationCurve()
                    //            {
                    //                IsAnimationEnabled = true,
                    //                WrapMode = AnimationWrapMode.PingPong,
                    //                TargetProperty = "Opacity",
                    //                Keyframes = new Keyframe[]
                    //                {
                    //                    new Keyframe() { Time = 0, Value = 0.75f },
                    //                    new Keyframe() { Time = 1, Value = 1f },
                    //                    new Keyframe() { Time = 2, Value = 1f },
                    //                    new Keyframe() { Time = 2.5f, Value = 0.5f },
                    //                }
                    //            }
                    //        }
                    //    },
                    //    new ImageLayer()
                    //    {
                    //        Name = "bg",
                    //        Source = "ms-appx:///Assets/death-star-1.jpg",
                    //        Transform = new TransformData(new Vector2(), new Vector2(450, 12), new Vector2(1, 1), 0),
                    //        Opacity = 0.76f,
                    //        Animations = new List<AnimationCurve>
                    //        {
                    //            new AnimationCurve()
                    //            {
                    //                IsAnimationEnabled = true,
                    //                WrapMode = AnimationWrapMode.PingPong,
                    //                TargetProperty = "Transform.Scale.X",
                    //                Keyframes = new Keyframe[]
                    //                {
                    //                    new Keyframe() { Time = 0, Value = 1 },
                    //                    new Keyframe() { Time = 2.5f, Value = 0.5f },
                    //                }
                    //            },
                    //            new AnimationCurve()
                    //            {
                    //                IsAnimationEnabled = true,
                    //                WrapMode = AnimationWrapMode.PingPong,
                    //                TargetProperty = "Transform.Scale.Y",
                    //                Keyframes = new Keyframe[]
                    //                {
                    //                    new Keyframe() { Time = 0, Value = 1 },
                    //                    new Keyframe() { Time = 2.5f, Value = 0.5f },
                    //                }
                    //            },
                    //            new AnimationCurve()
                    //            {
                    //                IsAnimationEnabled = true,
                    //                WrapMode = AnimationWrapMode.PingPong,
                    //                TargetProperty = "Opacity",
                    //                Keyframes = new Keyframe[]
                    //                {
                    //                    new Keyframe() { Time = 0, Value = 0.75f },
                    //                    new Keyframe() { Time = 1, Value = 1f },
                    //                    new Keyframe() { Time = 2, Value = 1f },
                    //                    new Keyframe() { Time = 2.5f, Value = 0.5f },
                    //                }
                    //            }
                    //        }
                    //    },
                    //    new ImageLayer()
                    //    {
                    //        Name = "bg2",
                    //        Source = "ms-appx:///Assets/anamorphic_flare2.png",
                    //        Transform = new TransformData(new Vector2(), new Vector2(22, 10), new Vector2(2, 1), 0),
                    //        BlendMode = CanvasBlend.Add
                    //    },
                    //    new TextLayer()
                    //    {
                    //        Name = "text",
                    //        Text = "Hello Renderer",
                    //        StartTime = 1,
                    //        Duration = 10,
                    //        Color = Color.FromArgb(255, 255, 255, 0),
                    //        Transform = new TransformData(new Vector2(0.5f,0.5f), new Vector2(450, 120), new Vector2(1, 1), 0),
                    //        Animations = new List<AnimationCurve>
                    //        {
                    //            new AnimationCurve()
                    //            {
                    //                IsAnimationEnabled = true,
                    //                WrapMode = AnimationWrapMode.PlayOnce,
                    //                TargetProperty = "Transform.Position.X",
                    //                Keyframes = new Keyframe[]
                    //                {
                    //                    new Keyframe() { Time = 0, Value = 450 },
                    //                    new Keyframe() { Time = 0.5f, Value = 20 },
                    //                    new Keyframe() { Time = 1, Value = 400 },
                    //                    new Keyframe() { Time = 2.5f, Value = 200 },
                    //                }
                    //            },
                    //            new AnimationCurve()
                    //            {
                    //                IsAnimationEnabled = true,
                    //                WrapMode = AnimationWrapMode.PlayOnce,
                    //                TargetProperty = "Transform.Position.Y",
                    //                Keyframes = new Keyframe[]
                    //                {
                    //                    new Keyframe() { Time = 0, Value = 120 },
                    //                    new Keyframe() { Time = 0.5f, Value = 40 },
                    //                    new Keyframe() { Time = 1, Value = 800 },
                    //                    new Keyframe() { Time = 2.5f, Value = 500 },
                    //                }
                    //            },
                    //            new AnimationCurve()
                    //            {
                    //                IsAnimationEnabled = true,
                    //                WrapMode = AnimationWrapMode.PlayOnce,
                    //                TargetProperty = "Transform.Rotation",
                    //                Keyframes = new Keyframe[]
                    //                {
                    //                    new Keyframe() { Time = 0, Value = 0 },
                    //                    new Keyframe() { Time = 3f, Value = 2*3.14f },
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            };
            return comp;
        }

        private async Task<FoundationComposition> CreateComp1(TransformData test)
        {
            var center = new Vector2(0.5f, 0.5f);

            var layers = new List<ILayer>
            {
                //new ImageLayer()
                //{
                //    Name = "bg",
                //    Source = "ms-appx:///Assets/Map/bg.jpg",
                //    Type = ImageType.Bitmap
                //},

                new PathLayer()
                {
                    Data = "M3.8,958.9c101.6-0.9,232.4-0.3,374.1,5.2c78.2,3,150.6,7.2,226.6,21.2c59.7,10.9,105.4,21.1,105.4,21.1c47.6,10.5,59.5,14.6,94.9,21.1c31.7,5.8,60.8,11.1,98.9,13c45.6,2.3,91.7,0.1,136.2-10.4c45.7-10.8,83.1-31.9,122.3-56.8c82.5-52.4,171.9-96.7,267.1-120.1c11.3-2.8,22.7-5.3,34.2-7.4c49.4-9.3,86.2-11,125.6-12.7c69-3.1,105-4.7,153.7,4.8c53.3,10.5,97.8,29.7,108,34.3c36.2,16,53.9,28.8,98.1,42.6c31.5,9.8,54.5,13.3,60,14c17.2,2.5,42.1,4.7,88.3,1.3c65.3-4.7,127.2-16.8,189-38.5c77.1-27,152.3-50.7,232.5-65.6c76.6-14.2,153.6-26.3,230.7-37.8c74.4-11.1,151.9-16.2,223.9-38.6c108.9-33.9,184.2-85.7,239.8-123.8c101.9-70,203.1-160.9,276.7-260.8c2.3-3.2,9.9-14.8,21.1-30.3c32.4-45.1,68.4-88,110.5-124.4c43.4-37.5,91.6-61.4,142.5-86.4c25.9-12.7,114-54.9,241.1-89.6c55-15,101.9-24.4,135.7-30.3",
                    DashStyle = CanvasDashStyle.Dot,
                    Transform = test,
                    DashOffset = 15f,
                    DashLen = 10f,
                    Thickness = 5f,
                    TrimEnd = 0,
                    Color = Color.FromArgb(255, 113, 60, 98),
                    Animations = new List<AnimationCurve>()
                        {
                            new AnimationCurve()
                            {
                                IsAnimationEnabled = true,
                                WrapMode = AnimationWrapMode.PlayOnce,
                                TargetProperty = "TrimEnd",
                                Keyframes = new Keyframe[]
                                {
                                    new Keyframe() { Time = 0, Value = 0 },
                                    new Keyframe() { Time = 6, Value = 1 },
                                }
                            },
                        }
                },

                new ImageLayer()
                {
                    Name = "icon1",
                    Source = "ms-appx:///Assets/Map/ugIcon.png",
                    Transform = new TransformData(center, new Vector2(1081, 1125), new Vector2(0), 0),
                    Type = ImageType.Bitmap,
                    StartTime = 1.4f,
                    Animations = new List<AnimationCurve>()
                    {
                        new AnimationCurve()
                        {
                            IsAnimationEnabled = true,
                            TargetProperty = "Transform.Scale",
                            WrapMode = AnimationWrapMode.PlayOnce,
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
                }
            };

            var comp = new FoundationComposition
            {
                FrameRate = 60,
                BackgroundColor = Color.FromArgb(0, 125, 125, 125),
                Layers = layers,
            };

            return comp;

        }
        private async Task<FoundationComposition> CreateComp3(TransformData test)
        {
            var center = new Vector2(0.5f, 0.5f);

            var layers = new List<ILayer>
            {
                //new ImageLayer()
                //{
                //    Name = "bg",
                //    Source = "ms-appx:///Assets/Map/bg.jpg",
                //    Type = ImageType.Bitmap
                //},

                new PathLayer()
                {
                    Data = "M3.8,958.9c101.6-0.9,232.4-0.3,374.1,5.2c78.2,3,150.6,7.2,226.6,21.2c59.7,10.9,105.4,21.1,105.4,21.1c47.6,10.5,59.5,14.6,94.9,21.1c31.7,5.8,60.8,11.1,98.9,13c45.6,2.3,91.7,0.1,136.2-10.4c45.7-10.8,83.1-31.9,122.3-56.8c82.5-52.4,171.9-96.7,267.1-120.1c11.3-2.8,22.7-5.3,34.2-7.4c49.4-9.3,86.2-11,125.6-12.7c69-3.1,105-4.7,153.7,4.8c53.3,10.5,97.8,29.7,108,34.3c36.2,16,53.9,28.8,98.1,42.6c31.5,9.8,54.5,13.3,60,14c17.2,2.5,42.1,4.7,88.3,1.3c65.3-4.7,127.2-16.8,189-38.5c77.1-27,152.3-50.7,232.5-65.6c76.6-14.2,153.6-26.3,230.7-37.8c74.4-11.1,151.9-16.2,223.9-38.6c108.9-33.9,184.2-85.7,239.8-123.8c101.9-70,203.1-160.9,276.7-260.8c2.3-3.2,9.9-14.8,21.1-30.3c32.4-45.1,68.4-88,110.5-124.4c43.4-37.5,91.6-61.4,142.5-86.4c25.9-12.7,114-54.9,241.1-89.6c55-15,101.9-24.4,135.7-30.3",
                    DashStyle = CanvasDashStyle.Dot,
                    Transform = test,
                    DashOffset = 15f,
                    DashLen = 10f,
                    Thickness = 5f,
                    TrimEnd = 0,
                    Color = Color.FromArgb(255, 113, 60, 98),
                    Animations = new List<AnimationCurve>()
                        {
                            new AnimationCurve()
                            {
                                IsAnimationEnabled = true,
                                WrapMode = AnimationWrapMode.PlayOnce,
                                TargetProperty = "TrimEnd",
                                Keyframes = new Keyframe[]
                                {
                                    new Keyframe() { Time = 0, Value = 0 },
                                    new Keyframe() { Time = 6, Value = 1 },
                                }
                            },
                        }
                },

                new ImageLayer()
                {
                    Name = "icon1",
                    Source = "ms-appx:///Assets/Map/ugIcon.png",
                    Transform = new TransformData(center, new Vector2(1081, 200), new Vector2(0), 0),
                    Type = ImageType.Bitmap,
                    StartTime = 1.4f,
                    Animations = new List<AnimationCurve>()
                    {
                        new AnimationCurve()
                        {
                            IsAnimationEnabled = true,
                            TargetProperty = "Transform.Scale",
                            WrapMode = AnimationWrapMode.PlayOnce,
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
                }
            };

            var comp = new FoundationComposition
            {
                FrameRate = 60,
                BackgroundColor = Color.FromArgb(0, 125, 125, 125),
                Layers = layers
            };

            return comp;

        }

        private void AnimController_FrameDispatched(object sender, EventArgs e)
        {
            renderer.SetCurrentTime(_animController.CurrentTime);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _animController.Seek(0);
            _animController.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RandomPage));
        }
    }
}
