using GhostCore.Animations.Core;
using GhostCore.Animations.Data;
using GhostCore.Animations.Data.Layers;
using GhostCore.Graphics.Colors;
using System.Collections.Generic;
using System.Numerics;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class TestProject : Project
    {
        public TestProject()
        {
            CreateTestAssets();
            Scenes = new List<Scene>
            {
                CreateTestScene(),
                CreateTestScene(),
            };
        }

        private void CreateTestAssets()
        {
            Assets = new List<ProjectAsset>
            {
                new FolderAsset
                {
                    Name = "Folder1",
                    Group = ColorGroup.Red,
                    SubItems = new List<ProjectAsset>
                    {
                        new FolderAsset{Name = "SubFolder1", Group = ColorGroup.Blue},
                        new FolderAsset{Name = "SubFolder2", Group = ColorGroup.Brown}
                    }
                },
                new FolderAsset
                {
                    Name = "Folder2",
                    Group = ColorGroup.Gray,
                    SubItems = new List<ProjectAsset>
                    {
                        new FolderAsset{Name = "SubFolder11"},
                        new FolderAsset
                        {
                            Group = ColorGroup.Green,
                            Name = "MainImages",
                            SubItems = new List<ProjectAsset>
                            {
                                new ImageAsset{Name = "main-image1.png", Path = System.IO.Path.GetTempPath()},
                                new ImageAsset{Name = "testcubical.png", Path = System.IO.Path.GetTempPath()},
                                new ImageAsset{Name = "3651831281 LOGO.png", Path = System.IO.Path.GetTempPath()},
                            }
                        },
                        new ImageAsset{Name = "Image1.jpg", Group = ColorGroup.Orange, Path = System.IO.Path.GetTempPath()},
                        new SvgAsset{Name = "someShape.svg", Path = System.IO.Path.GetTempPath()},
                        new ImageAsset{Name = "Image2.jpg", Path = System.IO.Path.GetTempPath()},
                        new SceneAsset { Name = "Scene1" },
                        new ImageAsset{Name = "Image3.png", Path = System.IO.Path.GetTempPath()},
                        new SvgAsset{Name = "logo.svg", Path = System.IO.Path.GetTempPath()},
                        new SceneAsset { Name = "Precomp2" },
                    },
                },
                new ImageAsset{Name = "Image1.jpg", Group = ColorGroup.Red, Path = System.IO.Path.GetTempPath()},
                new SvgAsset{Name = "someShape.svg", Group = ColorGroup.Violet, Path = System.IO.Path.GetTempPath()},
                new ImageAsset{Name = "Image2.jpg", Group = ColorGroup.White, Path = System.IO.Path.GetTempPath()},
                new SceneAsset { Name = "Scene1" , Group = ColorGroup.Yellow,},
                new SceneAsset { Name = "Precomp6" },
                new SceneAsset { Name = "Precomp010" },
            };
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
                    },
                    new ImageLayer()
                    {
                        StartTime = 1,
                        Duration = 1.2f,
                        Name = "bg2",
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
                Masks = new List<IMask>()
                {
                    new RectMask { RelativeOffset = new Vector2(0, 54), Width = 100, Height = 33 }
                },
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
                Masks = new List<IMask>()
                {
                    new RectMask{ RelativeOffset = new Vector2(0, 87), Width = 100, Height = 33 }
                },
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

            layers.Add(ugIcon);
            layers.Add(dlrIcon);
            layers.Add(bankText);
            layers.Add(btrem);

            layers.Add(compositeLayer);

            return scene;
        }
    }
}
