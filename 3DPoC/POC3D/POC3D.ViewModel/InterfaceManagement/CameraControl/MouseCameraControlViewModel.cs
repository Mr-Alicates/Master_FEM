using POC3D.ViewModel.Configuration;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.InterfaceManagement.CameraControl
{
    public class MouseCameraControlViewModel : BaseCameraControlViewModel
    {
        private Dictionary<MouseButton, Point> _pressedButtons = new Dictionary<MouseButton, Point>();
        private Point _currentMousePosition = new Point();
        private double _wheelDelta;

        public MouseCameraControlViewModel(ICameraViewModel cameraViewModel)
            : base(cameraViewModel)
        {
        }

        public void ReactToMouseWheelMovement(int delta)
        {
            _wheelDelta = delta / ApplicationConfiguration.MouseWheelSensitivity;
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

            if (_pressedButtons.ContainsKey(ApplicationConfiguration.PanMouseButton))
            {
                DoMousePanMovement();
            }
            else if (_pressedButtons.ContainsKey(ApplicationConfiguration.RotateMouseButton))
            {
                DoMouseRotation();
            }
            else if (_pressedButtons.ContainsKey(ApplicationConfiguration.OrbitMouseButton))
            {
                DoOrbitMovement();
            }
        }

        private void DoMouseWheelMovement()
        {
            if (_wheelDelta > 0)
            {
                _wheelDelta--;
                CameraViewModel.Move(CameraViewModel.UnaryForward * ApplicationConfiguration.MouseWheelDelta);
            }
            else if(_wheelDelta < 0)
            {
                _wheelDelta++;
                CameraViewModel.Move(-CameraViewModel.UnaryForward * ApplicationConfiguration.MouseWheelDelta);
            }
        }

        private void DoMousePanMovement()
        {
            var mousePositionWhenButtonPressed = _pressedButtons[ApplicationConfiguration.PanMouseButton];
            var vector = mousePositionWhenButtonPressed - _currentMousePosition;

            if (vector.Length != 0)
            {
                vector.Normalize();
            }

            var movementVector = CameraViewModel.UnaryUp * vector.Y + CameraViewModel.UnaryLeft * vector.X;

            CameraViewModel.Move(movementVector * ApplicationConfiguration.MousePanDelta);
        }

        private void DoMouseRotation()
        {
            var mousePositionWhenButtonPressed = _pressedButtons[ApplicationConfiguration.RotateMouseButton];
            var vector = mousePositionWhenButtonPressed - _currentMousePosition;

            if (vector.Length != 0)
            {
                vector.Normalize();
            }

            vector = vector * ApplicationConfiguration.MouseRotationDelta;

            CameraViewModel.Rotate(0, -vector.Y, vector.X);
        }

        private void DoOrbitMovement()
        {
            var sphereCenter = new Point3D(0, 0, 0);

            var mousePositionWhenButtonPressed = _pressedButtons[ApplicationConfiguration.OrbitMouseButton];
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
