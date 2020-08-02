using System.Windows.Input;
using System.Windows.Media;

namespace POC3D.ViewModel.Configuration
{
    public static class ApplicationConfiguration
    {
        private static ConfigurationViewModel _applicationConfiguration;

        public static ConfigurationViewModel Configuration => _applicationConfiguration ??= BuildDefaultConfiguration();

        private static ConfigurationViewModel BuildDefaultConfiguration()
        {
            return new ConfigurationViewModel()
            {
                GraphicsObjectSizeMultiplier = 0.1,

                BarBrush = Brushes.Blue,
                SelectedBarBrush = Brushes.Red,

                ForceBrush = Brushes.Yellow,
                SelectedForceBrush = Brushes.Red,

                FreeNodeBrush = Brushes.LightGreen,
                FixedNodeBrush = Brushes.DarkGreen,
                SelectedNodeBrush = Brushes.Red,

                MouseRotationDelta = 0.5,
                MouseWheelSensitivity = 20,

                KeyboardRotationDelta = 0.5,

                PanMouseButton = MouseButton.Left,
                RotateMouseButton = MouseButton.Middle,
                OrbitMouseButton = MouseButton.Right,

                ForwardKey = Key.W,
                BackwardKey = Key.S,
                LeftKey = Key.A,
                RightKey = Key.D,
                UpKey = Key.R,
                DownKey = Key.F,
                PitchUpKey = Key.W,
                PitchDownKey = Key.S,
                YawUpKey = Key.A,
                YawDownKey = Key.D,
                RollUpKey = Key.E,
                RollDownKey = Key.Q,
                SpecialKey = Key.LeftShift,
            };
        }

        public static double GraphicsObjectSizeMultiplier => Configuration.GraphicsObjectSizeMultiplier;

        public static Brush BarBrush => Configuration.BarBrush;
        public static Brush SelectedBarBrush => Configuration.SelectedBarBrush;

        public static Brush ForceBrush => Configuration.ForceBrush;
        public static Brush SelectedForceBrush => Configuration.SelectedForceBrush;

        public static Brush FreeNodeBrush => Configuration.FreeNodeBrush;
        public static Brush FixedNodeBrush => Configuration.FixedNodeBrush;
        public static Brush SelectedNodeBrush => Configuration.SelectedNodeBrush;

        public static double MouseRotationDelta => Configuration.MouseRotationDelta;
        public static double MouseWheelSensitivity => Configuration.MouseWheelSensitivity;

        public static double KeyboardRotationDelta => Configuration.KeyboardRotationDelta;

        public static MouseButton PanMouseButton => Configuration.PanMouseButton;
        public static MouseButton RotateMouseButton => Configuration.RotateMouseButton;
        public static MouseButton OrbitMouseButton => Configuration.OrbitMouseButton;

        public static Key ForwardKey => Configuration.ForwardKey;
        public static Key BackwardKey => Configuration.BackwardKey;
        public static Key LeftKey => Configuration.LeftKey;
        public static Key RightKey => Configuration.RightKey;
        public static Key UpKey => Configuration.UpKey;
        public static Key DownKey => Configuration.DownKey;
        public static Key PitchUpKey => Configuration.PitchUpKey;
        public static Key PitchDownKey => Configuration.PitchDownKey;
        public static Key YawUpKey => Configuration.YawUpKey;
        public static Key YawDownKey => Configuration.YawDownKey;
        public static Key RollUpKey => Configuration.RollUpKey;
        public static Key RollDownKey => Configuration.RollDownKey;
        public static Key SpecialKey => Configuration.SpecialKey;
    }
}
