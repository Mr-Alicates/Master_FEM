using POC3D.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.InterfaceManagement
{
    public class CameraViewModel : Observable
    {        
        private Point3D _position = new Point3D();
        private Vector3D _forwardVector = new Vector3D(1,0,0);
        private Vector3D _upVector = new Vector3D(0, 0, 1);

        #region Keyboard movement

        private HashSet<Key> _pressedKeys = new HashSet<Key>();
        private Vector3D _keyboardPositionDeltaVector = new Vector3D();
        private static readonly Key _forward = Key.W;
        private static readonly Key _backward = Key.S;
        private static readonly Key _left = Key.A;
        private static readonly Key _right = Key.D;
        private static readonly Key _up = Key.R;
        private static readonly Key _down = Key.F;
        private static readonly Key _pitchUp = Key.W;
        private static readonly Key _pitchDown = Key.S;
        private static readonly Key _yawUp = Key.A;
        private static readonly Key _yawDown = Key.D;
        private static readonly Key _rollUp = Key.E;
        private static readonly Key _rollDown = Key.Q;
        private static readonly Key _special = Key.LeftShift;
        private static readonly double _keyboardRotationDelta = 0.5;
        private double _keyboardRotationX;
        private double _keyboardRotationY;
        private double _keyboardRotationZ;

        #endregion

        #region Mouse Wheel

        private Vector3D _wheelMovementVector = new Vector3D();
        private int _wheelDelta;

        #endregion

        #region Mouse movement

        private Dictionary<MouseButton, Point> _pressedButtons = new Dictionary<MouseButton, Point>();
        private Vector3D _mouseMovementVector = new Vector3D();
        private Point _currentMousePosition = new Point();
        private MouseButton _panMouseButton = MouseButton.Middle;
        private MouseButton _rotateMouseButton = MouseButton.Left;
        private static readonly double _mouseRotationDelta = 0.5;
        private double _mouseRotationY;
        private double _mouseRotationZ;

        #endregion

        #region Orbiting

        private MouseButton _orbitMouseButton = MouseButton.Right;

        #endregion 

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
                UpdateKeyboardRotation();
                UpdateMouseRotation();

                var rotationX = _keyboardRotationX;
                var rotationY = _keyboardRotationY + _mouseRotationY;
                var rotationZ = _keyboardRotationZ + _mouseRotationZ;

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

                UpdateKeyboardMovement();
                UpdateMouseWheelMovement();
                UpdateMousePan();

                var positionDelta = _keyboardPositionDeltaVector + _mouseMovementVector + _wheelMovementVector;
                Position += positionDelta;

                DoOrbitMovement();

                await Task.Delay(1);
            }
        }

        public string FriendlyPosition => $"Camera Position ({Position.X:0.##}/{Position.Y:0.##}/{Position.Z:0.##})";

        public string FriendlyLookDirection =>
            $"Look Direction ({UnaryForward.X:0.##}/{UnaryForward.Y:0.##}/{UnaryForward.Z:0.##})";

        public Vector3D UpDirection { get; }

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

        public Vector3D UnaryUp
        {
            get => _upVector;
            set
            {
                _upVector = value;
                OnCameraViewModelChanged?.Invoke(this, null);
            }
        }

        public Vector3D UnaryLeft => Vector3D.CrossProduct(UnaryUp, UnaryForward);

        public Vector3D UnaryForward
        {
            get => _forwardVector;
            set
            {
                if(value == null || value.Length == 0)
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

        #region Keyboard

        public void ReactToKeyboardKeyDown(Key pressedKey)
        {
            _pressedKeys.Add(pressedKey);
        }

        public void ReactToKeyboardKeyUp(Key liftedKey)
        {
            _pressedKeys.Remove(liftedKey);
        }

        private void UpdateKeyboardMovement()
        {
            _keyboardPositionDeltaVector = new Vector3D();

            if (_pressedKeys.Contains(_special))
            {
                return;
            }

            var delta = new Vector3D();

            if (_pressedKeys.Contains(_forward))
            {
                delta += UnaryForward;
            }
            if (_pressedKeys.Contains(_backward))
            {
                delta -= UnaryForward;
            }
            if (_pressedKeys.Contains(_left))
            {
                delta += UnaryLeft;
            }
            if (_pressedKeys.Contains(_right))
            {
                delta -= UnaryLeft;
            }
            if (_pressedKeys.Contains(_up))
            {
                delta += UnaryUp;
            }
            if (_pressedKeys.Contains(_down))
            {
                delta -= UnaryUp;
            }

            _keyboardPositionDeltaVector = delta;
        }

        private void UpdateKeyboardRotation()
        {
            _keyboardRotationX = 0;
            _keyboardRotationY = 0;
            _keyboardRotationZ = 0;

            if (!_pressedKeys.Contains(_special))
            {
                return;
            }

            if (_pressedKeys.Contains(_pitchUp))
            {
                _keyboardRotationY = _keyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_pitchDown))
            {
                _keyboardRotationY = -_keyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_yawUp))
            {
                _keyboardRotationZ = _keyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_yawDown))
            {
                _keyboardRotationZ = -_keyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_rollUp))
            {
                _keyboardRotationX = _keyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_rollDown))
            {
                _keyboardRotationX = -_keyboardRotationDelta;
            }
        }

        #endregion

        #region MouseWheel

        public void ReactToMouseWheelMovement(int delta)
        {
            _wheelDelta = delta / 5;
        }

        private void UpdateMouseWheelMovement()
        {
            if (_wheelDelta != 0)
            {
                if (_wheelDelta > 0)
                {
                    _wheelDelta--;
                    _wheelMovementVector = UnaryForward;
                }
                else
                {
                    _wheelDelta++;
                    _wheelMovementVector = -UnaryForward;
                }
            }
            else
            {
                _wheelMovementVector = new Vector3D();
            }
        }

        #endregion

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

        private void UpdateMousePan()
        {
            _mouseMovementVector.Y = 0;
            _mouseMovementVector.Z = 0;

            if (!_pressedButtons.ContainsKey(_panMouseButton))
            {
                return;
            }

            var mousePositionWhenButtonPressed = _pressedButtons[_panMouseButton];
            var vector = mousePositionWhenButtonPressed - _currentMousePosition;

            _mouseMovementVector.Y = vector.X;
            _mouseMovementVector.Z = vector.Y;

            if (_mouseMovementVector.Length != 0)
            {
                _mouseMovementVector.Normalize();
            }
        }

        private void UpdateMouseRotation()
        {
            _mouseRotationY = 0;
            _mouseRotationZ = 0;

            if (!_pressedButtons.ContainsKey(_rotateMouseButton))
            {
                return;
            }

            var mousePositionWhenButtonPressed = _pressedButtons[_rotateMouseButton];
            var vector = mousePositionWhenButtonPressed - _currentMousePosition;

            if (vector.Length != 0)
            {
                vector.Normalize();
            }

            vector = vector * _mouseRotationDelta;

            _mouseRotationY = -vector.Y;
            _mouseRotationZ = vector.X;
        }

        private void DoOrbitMovement()
        {
            if (!_pressedButtons.ContainsKey(_orbitMouseButton))
            {
                return;
            }

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
                        new RotateTransform3D(new AxisAngleRotation3D(UnaryLeft, rotationY), sphereCenter),
                        new RotateTransform3D(new AxisAngleRotation3D(UnaryUp, rotationZ), sphereCenter)
                    }
            };

            //This does the orbiting
            Position = transformGroup.Transform(Position);

            //This makes the camera face the center of the sphere
            UnaryForward = sphereCenter - Position;
            UnaryUp = Vector3D.CrossProduct(UnaryForward, UnaryLeft);
        }
    }
}