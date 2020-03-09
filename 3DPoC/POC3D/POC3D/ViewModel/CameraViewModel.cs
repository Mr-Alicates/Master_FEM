using System;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class CameraViewModel : Observable
    {
        private const int RotationDelta = 1;

        private int _cameraRotationY;
        private int _cameraRotationZ;
        private Point3D _position;

        public EventHandler OnCameraViewModelChanged;

        public CameraViewModel()
        {
            UpDirection = new Vector3D(0, 0, 1);
        }

        public string FriendlyPosition => $"Camera Position ({Position.X:0.##}/{Position.Y:0.##}/{Position.Z:0.##})";

        public string FriendlyLookDirection =>
            $"Look Direction ({UnaryForward.X:0.##}/{UnaryForward.Y:0.##}/{UnaryForward.Z:0.##})";

        public Vector3D UpDirection { get; }

        public int CameraRotationY
        {
            get => _cameraRotationY;
            protected set
            {
                _cameraRotationY = value;
                OnPropertyChanged(nameof(FriendlyLookDirection));
                OnCameraViewModelChanged?.Invoke(this, null);
            }
        }

        public int CameraRotationZ
        {
            get => _cameraRotationZ;
            protected set
            {
                _cameraRotationZ = value;
                OnPropertyChanged(nameof(FriendlyLookDirection));
                OnCameraViewModelChanged?.Invoke(this, null);
            }
        }

        public Point3D Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(FriendlyPosition));
                OnCameraViewModelChanged?.Invoke(this, null);
            }
        }

        public Vector3D UnaryUp => new Vector3D(0, 0, 1);

        public Vector3D UnaryLeft
        {
            get
            {
                var forward = new Vector3D(UnaryForward.X, UnaryForward.Y, 0);
                forward.Normalize();

                var transformGroup = new Transform3DGroup
                {
                    Children = new Transform3DCollection
                    {
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90))
                    }
                };

                var left = transformGroup.Transform(forward);
                left.Normalize();
                return left;
            }
        }

        public Vector3D UnaryForward
        {
            get
            {
                var vector = new Vector3D(1, 0, 0);

                var transformGroup = new Transform3DGroup
                {
                    Children = new Transform3DCollection
                    {
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), CameraRotationY)),
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), CameraRotationZ))
                    }
                };

                var transformedVector = transformGroup.Transform(vector);
                transformedVector.Normalize();

                return transformedVector;
            }
        }

        #region CameraRotation

        public void CameraRotationZUp()
        {
            CameraRotationZ = CameraRotationZ + RotationDelta;
        }

        public void CameraRotationZDown()
        {
            CameraRotationZ = CameraRotationZ - RotationDelta;
        }

        public void CameraRotationYUp()
        {
            CameraRotationY = CameraRotationY + RotationDelta;
        }

        public void CameraRotationYDown()
        {
            CameraRotationY = CameraRotationY - RotationDelta;
        }

        #endregion

        #region Movement

        public void MoveForward()
        {
            Position = Position + UnaryForward;
        }

        public void MoveBackwards()
        {
            Position = Position - UnaryForward;
        }

        public void MoveUp()
        {
            Position = Position + UnaryUp;
        }

        public void MoveDown()
        {
            Position = Position - UnaryUp;
        }

        public void MoveLeft()
        {
            Position = Position + UnaryLeft;
        }

        public void MoveRight()
        {
            Position = Position - UnaryLeft;
        }

        #endregion
    }
}