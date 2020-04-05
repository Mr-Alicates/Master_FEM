using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.InterfaceManagement.CameraControl
{
    public class MouseCameraControlViewModel : BaseCameraControlViewModel
    {
        private const double MouseRotationDelta = 0.5;
        private const int MouseWheelSensitivity = 5;

        private Dictionary<MouseButton, Point> _pressedButtons = new Dictionary<MouseButton, Point>();
        private Point _currentMousePosition = new Point();
        private double _wheelDelta;

        private MouseButton _panMouseButton = MouseButton.Left;
        private MouseButton _rotateMouseButton = MouseButton.Middle;
        private MouseButton _orbitMouseButton = MouseButton.Right;

        public MouseCameraControlViewModel(ICameraViewModel cameraViewModel)
            : base(cameraViewModel)
        {
        }

        public void ReactToMouseWheelMovement(int delta)
        {
            _wheelDelta = delta / MouseWheelSensitivity;
        }

        public void ReactToMouseDown(MouseButton mouseButton)
        {
            if (_pressedButtons.ContainsKey(mouseButton))
            {
                _pressedButtons[mouseButton] = _currentMousePosition;
            }
            else
            {
                _pressedButtons.Add(mouseButton, _currentMousePosition);
            }
        }

        public void ReactToMouseUp(MouseButton mouseButton)
        {
            _pressedButtons.Remove(mouseButton);
        }

        public void ReactToMouseMovement(Point currentCursorPosition)
        {
            _currentMousePosition = currentCursorPosition;
        }

        public override void UpdateCamera()
        {
            DoMouseWheelMovement();

            if (_pressedButtons.ContainsKey(_panMouseButton))
            {
                DoMousePanMovement();
            }
            else if (_pressedButtons.ContainsKey(_rotateMouseButton))
            {
                DoMouseRotation();
            }
            else if (_pressedButtons.ContainsKey(_orbitMouseButton))
            {
                DoOrbitMovement();
            }
        }

        private void DoMouseWheelMovement()
        {
            if (_wheelDelta > 0)
            {
                _wheelDelta--;
                CameraViewModel.Move(CameraViewModel.UnaryForward);
            }
            else if(_wheelDelta < 0)
            {
                _wheelDelta++;
                CameraViewModel.Move(-CameraViewModel.UnaryForward);
            }
        }

        private void DoMousePanMovement()
        {
            var mousePositionWhenButtonPressed = _pressedButtons[_panMouseButton];
            var vector = mousePositionWhenButtonPressed - _currentMousePosition;

            var mouseMovementVector = new Vector3D(0, vector.X, vector.Y);

            CameraViewModel.Move(mouseMovementVector);
        }

        private void DoMouseRotation()
        {
            var mousePositionWhenButtonPressed = _pressedButtons[_rotateMouseButton];
            var vector = mousePositionWhenButtonPressed - _currentMousePosition;

            if (vector.Length != 0)
            {
                vector.Normalize();
            }

            vector = vector * MouseRotationDelta;

            CameraViewModel.Rotate(0, -vector.Y, vector.X);
        }

        private void DoOrbitMovement()
        {
            var sphereCenter = new Point3D(0, 0, 0);

            var mousePositionWhenButtonPressed = _pressedButtons[_orbitMouseButton];
            var vector = mousePositionWhenButtonPressed - _currentMousePosition;

            if (vector.Length != 0)
            {
                vector.Normalize();
            }

            var rotationY = -vector.Y;
            var rotationZ = vector.X;

            var transformGroup = new Transform3DGroup
            {
                Children = new Transform3DCollection
                    {
                        new RotateTransform3D(new AxisAngleRotation3D(CameraViewModel.UnaryLeft, rotationY), sphereCenter),
                        new RotateTransform3D(new AxisAngleRotation3D(CameraViewModel.UnaryUp, rotationZ), sphereCenter)
                    }
            };

            //This does the orbiting
            var newPosition = transformGroup.Transform(CameraViewModel.Position);
            CameraViewModel.Move(newPosition);

            //This makes the camera face the center of the sphere
            CameraViewModel.LookAt(sphereCenter);
        }
    }
}
