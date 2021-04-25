using GhostCore.Animations.Data;
using System.Collections.Generic;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class Project
    {
        public List<ProjectAsset> Assets { get; set; }
        public List<Scene> Scenes { get; internal set; }

        public Project()
        {
            Assets = new List<ProjectAsset>();
            Scenes = new List<Scene>();
        }
    }
}
