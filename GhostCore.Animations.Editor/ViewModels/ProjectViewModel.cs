using GhostCore.MVVM;
using System.Collections.Generic;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        public List<ProjectAsset> Assets { get; set; }

        public ProjectViewModel()
        {
            // Test
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
    }
}
