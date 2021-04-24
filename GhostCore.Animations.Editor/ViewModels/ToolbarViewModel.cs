using GhostCore.MVVM;
using System.Collections.Generic;
using System.Linq;

namespace GhostCore.Animations.Editor.ViewModels
{
    public class ToolbarViewModel : ViewModelBase
    {
        private Dictionary<string, bool> _toggleToolStates;

        public bool IsMoveToolSelected
        {
            get { return _toggleToolStates[nameof(IsMoveToolSelected)]; }
            set { _toggleToolStates[nameof(IsMoveToolSelected)] = value; UpdateAllToolProperties(nameof(IsMoveToolSelected)); }
        }

        public bool IsPanToolSelected
        {
            get { return _toggleToolStates[nameof(IsPanToolSelected)]; }
            set { _toggleToolStates[nameof(IsPanToolSelected)] = value; UpdateAllToolProperties(nameof(IsPanToolSelected)); }
        }

        public bool IsZoomToolSelected
        {
            get { return _toggleToolStates[nameof(IsZoomToolSelected)]; }
            set { _toggleToolStates[nameof(IsZoomToolSelected)] = value; UpdateAllToolProperties(nameof(IsZoomToolSelected)); }
        }

        public bool IsRotateToolSelected
        {
            get { return _toggleToolStates[nameof(IsRotateToolSelected)]; }
            set { _toggleToolStates[nameof(IsRotateToolSelected)] = value; UpdateAllToolProperties(nameof(IsRotateToolSelected)); }
        }

        public bool IsScaleToolSelected
        {
            get { return _toggleToolStates[nameof(IsScaleToolSelected)]; }
            set { _toggleToolStates[nameof(IsScaleToolSelected)] = value; UpdateAllToolProperties(nameof(IsScaleToolSelected)); }
        }

        public bool IsMaskToolSelected
        {
            get { return _toggleToolStates[nameof(IsMaskToolSelected)]; }
            set { _toggleToolStates[nameof(IsMaskToolSelected)] = value; UpdateAllToolProperties(nameof(IsMaskToolSelected)); }
        }

        public bool IsPenToolSelected
        {
            get { return _toggleToolStates[nameof(IsPenToolSelected)]; }
            set { _toggleToolStates[nameof(IsPenToolSelected)] = value; UpdateAllToolProperties(nameof(IsPenToolSelected)); }
        }

        public bool IsTextToolSelected
        {
            get { return _toggleToolStates[nameof(IsTextToolSelected)]; }
            set { _toggleToolStates[nameof(IsTextToolSelected)] = value; UpdateAllToolProperties(nameof(IsTextToolSelected)); }
        }
        public ToolbarViewModel()
        {
            _toggleToolStates = new Dictionary<string, bool>
            {
                { nameof(IsMoveToolSelected), true },
                { nameof(IsTextToolSelected), false },
                { nameof(IsPenToolSelected), false },
                { nameof(IsMaskToolSelected), false },
                { nameof(IsScaleToolSelected), false },
                { nameof(IsRotateToolSelected), false },
                { nameof(IsZoomToolSelected), false },
                { nameof(IsPanToolSelected), false },
            };
        }

        private void UpdateAllToolProperties(string propName)
        {
            foreach (var item in _toggleToolStates.ToList())
            {
                if (item.Key == propName)
                    continue;

                _toggleToolStates[item.Key] = false;
                OnPropertyChanged(item.Key);
            }
        }
    }
}
