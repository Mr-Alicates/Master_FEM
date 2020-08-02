using POC3D.ViewModel.Base;
using System;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.InterfaceManagement.CameraControl
{
    public class CameraViewModel : Observable, ICameraViewModel
    {
        private Point3D _position = new Point3D();
        private Vector3D _forwardVector = new Vector3D(1, 0, 0);
        private Vector3D _upVector = new Vector3D(0, 0, 1);

        public CameraViewModel()
        {
            Move(new Point3D(5, 5, 5));
            LookAt(new Point3D(0, 0, 0));
        }

        public EventHandler OnCameraViewModelChanged;

        public string FriendlyPosition => $"Camera Position ({Position.X:0.##}/{Position.Y:0.##}/{Position.Z:0.##})";

        public string FriendlyLookDirection => $"Look Direction ({UnaryForward.X:0.##}/{UnaryForward.Y:0.##}/{UnaryForward.Z:0.##})";

        public Point3D Position
        {
            get => _position;
            private set
            {
                _position = value;
                OnPropertyChanged(nameof(FriendlyPosition));
                OnCameraViewModelChanged?.Invoke(this, null);
            }
        }

        public Vector3D UnaryUp
        {
            get => _upVector;
            set
            {
                if (value == null || value.Length == 0)
                {
                    _upVector = new Vector3D(1, 0, 0);
                }
                else
                {
                    _upVector = value;
                    _upVector.Normalize();
                }

                OnCameraViewModelChanged?.Invoke(this, null);
            }
        }

        public Vector3D UnaryLeft => Vector3D.CrossProduct(UnaryUp, UnaryForward);

        public Vector3D UnaryForward
        {
            get => _forwardVector;
            private set
            {
                if (value == null || value.Length == 0)
                {
                    _forwardVector = new Vector3D(1, 0, 0);
                }
                else
                {
                    _forwardVector = value;
                    _forwardVector.Normalize();
                }

                OnPropertyChanged(nameof(FriendlyLookDirection));
                OnCameraViewModelChanged?.Invoke(this, null);
            }
        }

        public void Rotate(double rotationX, double rotationY, double rotationZ)
        {
            var transformGroup = new Transform3DGroup
            {
                Children = new Transform3DCollection
                {
                    new RotateTransform3D(new AxisAngleRotation3D(UnaryForward, rotationX), Position),
                    new RotateTransform3D(new AxisAngleRotation3D(UnaryLeft, rotationY), Position),
                    new RotateTransform3D(new AxisAngleRotation3D(UnaryUp, rotationZ), Position)
                }
            };

            //This does the "turning" of the camera
            UnaryUp = transformGroup.Transform(UnaryUp);
            UnaryForward = transformGroup.Transform(UnaryForward);
        }

        public void Move(Vector3D delta)
        {
            if (delta.Length != 0)
            {
                delta.Normalize();
                Position += delta;
            }
        }

        public void Move(Point3D newPosition)
        {
            Position = newPosition;
        }

        public void LookAt(Point3D lookAtPoint)
        {
            UnaryForward = lookAtPoint - Position;
            UnaryUp = Vector3D.CrossProduct(UnaryForward, UnaryLeft);
        }
    }
}