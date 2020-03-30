using POC3D.ViewModel.Base;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.InterfaceManagement
{
    public class CameraViewModel : Observable
    {
        private const int RotationDelta = 1;

        private int _cameraRotationY;
        private int _cameraRotationZ;
        private Point3D _position;

        private Vector3D _movementVector = new Vector3D();

        public EventHandler OnCameraViewModelChanged;

        public CameraViewModel()
        {
            UpDirection = new Vector3D(0, 0, 1);
            Application.Current.Dispatcher.InvokeAsync(UpdateCamera);
        }

        private async Task UpdateCamera()
        {
            while (true)
            {
                if (_movementVector.Length != 0)
                {
                    Position = Position + _movementVector;
                }

                await Task.Delay(1);
            }
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

        private Point _lastMousePosition;

        public void ReactToMouseWheelMovement(int delta)
        {
            var movements = Math.Abs(delta / 10);

            for (var i = 0; i < movements; i++)
            {
                if (delta > 0) MoveForward();

                if (delta < 0) MoveBackwards();
            }
        }

        public void ReactToMouseMovement(
            MouseButtonState middleButton,
            MouseButtonState rightButton,
            Point currentCursorPosition)
        {
            if (middleButton == MouseButtonState.Pressed) ReactToCameraPanMouse(currentCursorPosition);

            if (rightButton == MouseButtonState.Pressed) ReactToCameraRotateMouse(currentCursorPosition);

            _lastMousePosition = currentCursorPosition;
        }

        public void ReactToKeyBoardKeyDown(bool isLeftShiftPressed, Key pressedKey)
        {
            if (isLeftShiftPressed)
                ReactToCameraRotationKeyDown(pressedKey);
            else
                ReactToMovementKeyDown(pressedKey);
        }

        private void ReactToCameraRotateMouse(Point currentCursorPosition)
        {
            if (currentCursorPosition.X < _lastMousePosition.X) CameraRotationZUp();

            if (currentCursorPosition.X > _lastMousePosition.X) CameraRotationZDown();

            if (currentCursorPosition.Y < _lastMousePosition.Y) CameraRotationYDown();

            if (currentCursorPosition.Y > _lastMousePosition.Y) CameraRotationYUp();
        }

        private void ReactToCameraPanMouse(Point currentCursorPosition)
        {
            if (currentCursorPosition.X < _lastMousePosition.X) MoveLeft();

            if (currentCursorPosition.X > _lastMousePosition.X) MoveRight();

            if (currentCursorPosition.Y < _lastMousePosition.Y) MoveUp();

            if (currentCursorPosition.Y > _lastMousePosition.Y) MoveDown();
        }

        private void ReactToCameraRotationKeyDown(Key pressedKey)
        {
            switch (pressedKey)
            {
                case Key.A:
                    CameraRotationZUp();
                    break;
                case Key.D:
                    CameraRotationZDown();
                    break;
                case Key.S:
                    CameraRotationYUp();
                    break;
                case Key.W:
                    CameraRotationYDown();
                    break;
            }
        }

        private void ReactToMovementKeyDown(Key pressedKey)
        {
            switch (pressedKey)
            {
                case Key.A:
                    MoveLeft();
                    break;
                case Key.D:
                    MoveRight();
                    break;
                case Key.W:
                    MoveForward();
                    break;
                case Key.S:
                    MoveBackwards();
                    break;
                case Key.R:
                    MoveUp();
                    break;
                case Key.F:
                    MoveDown();
                    break;
            }
        }

    }
}