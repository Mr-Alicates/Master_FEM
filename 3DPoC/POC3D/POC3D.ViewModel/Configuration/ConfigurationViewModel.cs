using POC3D.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace POC3D.ViewModel.Configuration
{
    public class ConfigurationViewModel : Observable
    {
        private double _graphicsObjectSizeMultiplier;

        public double GraphicsObjectSizeMultiplier
        {
            get => _graphicsObjectSizeMultiplier;
            set
            {
                _graphicsObjectSizeMultiplier = value;
                OnPropertyChanged(nameof(GraphicsObjectSizeMultiplier));
            }
        }

        public Brush BarBrush { get; set; }
        public Brush SelectedBarBrush { get; set; }

        public Brush ForceBrush { get; set; }
        public Brush SelectedForceBrush { get; set; }

        public Brush FreeNodeBrush { get; set; }
        public Brush FixedNodeBrush { get; set; }
        public Brush SelectedNodeBrush { get; set; }

        public double MouseRotationDelta { get; set; }
        public double MouseWheelSensitivity { get; set; }
        public double MousePanDelta { get; set; }
        public double MouseWheelDelta { get; set; }

        public double KeyboardRotationDelta { get; set; }

        public MouseButton PanMouseButton { get; set; }
        public MouseButton RotateMouseButton { get; set; }
        public MouseButton OrbitMouseButton { get; set; }

        public Key ForwardKey { get; set; }
        public Key BackwardKey { get; set; }
        public Key LeftKey { get; set; }
        public Key RightKey { get; set; }
        public Key UpKey { get; set; }
        public Key DownKey { get; set; }
        public Key PitchUpKey { get; set; }
        public Key PitchDownKey { get; set; }
        public Key YawUpKey { get; set; }
        public Key YawDownKey { get; set; }
        public Key RollUpKey { get; set; }
        public Key RollDownKey { get; set; }
        public Key SpecialKey { get; set; }
    }
}
