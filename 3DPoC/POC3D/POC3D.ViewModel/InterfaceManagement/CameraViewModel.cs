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
        private const int RotationDelta = 1;

        private double _cameraRotationY;
        private double _cameraRotationZ;
        private Point3D _position;


        #region Keyboard movement

        private HashSet<Key> _pressedKeys = new HashSet<Key>();
        private Vector3D _keyboardMovementVector = new Vector3D();
        private double _keyboardRotationY;
        private double _keyboardRotationZ;
        private static readonly Key _forward = Key.W;
        private static readonly Key _backward = Key.S;
        private static readonly Key _left = Key.A;
        private static readonly Key _right = Key.D;
        private static readonly Key _up = Key.R;
        private static readonly Key _down = Key.F;
        private static readonly Key _special = Key.LeftShift;
        private static readonly double _keyboardRotationDelta = 0.5;

        #endregion

        #region Mouse Wheel

        #endregion

        private Vector3D _wheelMovementVector = new Vector3D();
        private int _wheelDelta;


        #region Mouse movement

        private Dictionary<MouseButton, Point> _pressedButtons = new Dictionary<MouseButton, Point>(); 
        private Vector3D _mouseMovementVector = new Vector3D();
        private Point _currentMousePosition = new Point();
        private MouseButton _panMouseButton = MouseButton.Middle;
        private MouseButton _rotateMouseButton = MouseButton.Right;
        private double _mouseRotationY;
        private double _mouseRotationZ;

        #endregion


        public EventHandler OnCameraViewModelChanged;

        public CameraViewModel()
        {
            UpDirection = new Vector3D(0, 0, 1);
            Application.Current.Dispatcher.InvokeAsync(UpdateCamera);
        }

        private async Task UpdateCamera()
        {
            const double multiplier = 0.5;

            while (true)
            {
                UpdateKeyboardRotation();
                UpdateMouseRotation();

                CameraRotationY += _keyboardRotationY + _mouseRotationY;
                CameraRotationZ += _keyboardRotationZ + _mouseRotationZ;

                UpdateKeyboardMovement();
                UpdateMouseWheelMovement();
                UpdateMouseMovement();

                var _movementVector = _keyboardMovementVector + _mouseMovementVector + _wheelMovementVector;
                
                if (_movementVector.Length != 0)
                {
                    Position += _movementVector * multiplier;
                }

                await Task.Delay(1);
            }
        }

        public string FriendlyPosition => $"Camera Position ({Position.X:0.##}/{Position.Y:0.##}/{Position.Z:0.##})";

        public string FriendlyLookDirection =>
            $"Look Direction ({UnaryForward.X:0.##}/{UnaryForward.Y:0.##}/{UnaryForward.Z:0.##})";

        public Vector3D UpDirection { get; }

        public double CameraRotationY
        {
            get => _cameraRotationY;
            protected set
            {
                _cameraRotationY = value;
                OnPropertyChanged(nameof(FriendlyLookDirection));
                OnCameraViewModelChanged?.Invoke(this, null);
            }
        }

        public double CameraRotationZ
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


        #region Keyboard

        public void ReactToKeyboardKeyDown(Key pressedKey)
        {
            _pressedKeys.Add(pressedKey);
        }

        public void ReactToKeyboardKeyUp(Key liftedKey)
        {
            _pressedKeys.Remove(liftedKey);
        }

        private void UpdateKeyboardRotation()
        {
            _keyboardRotationY = 0;
            _keyboardRotationZ = 0;

            if (!_pressedKeys.Contains(_special))
            {
                return;
            }

            if (_pressedKeys.Contains(_forward))
            {
                _keyboardRotationY -= _keyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_backward))
            {
                _keyboardRotationY += _keyboardRotationDelta;
            }

            if (_pressedKeys.Contains(_left))
            {
                _keyboardRotationZ += _keyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_right))
            {
                _keyboardRotationZ -= _keyboardRotationDelta;
            }
        }

        private void UpdateKeyboardMovement()
        {
            _keyboardMovementVector = new Vector3D();

            if (_pressedKeys.Contains(_special))
            {
                return;
            }

            if (_pressedKeys.Contains(_forward))
            {
                _keyboardMovementVector += UnaryForward;
            }
            if (_pressedKeys.Contains(_backward))
            {
                _keyboardMovementVector -= UnaryForward;
            }

            if (_pressedKeys.Contains(_left))
            {
                _keyboardMovementVector += UnaryLeft;
            }
            if (_pressedKeys.Contains(_right))
            {
                _keyboardMovementVector -= UnaryLeft;
            }

            if (_pressedKeys.Contains(_up))
            {
                _keyboardMovementVector += UnaryUp;
            }
            if (_pressedKeys.Contains(_down))
            {
                _keyboardMovementVector -= UnaryUp;
            }

            if (_keyboardMovementVector.Length > 0) 
            {
                _keyboardMovementVector.Normalize();
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

        private void UpdateMouseMovement()
        {
            _mouseMovementVector.Y = 0;
            _mouseMovementVector.Z = 0;

            if (!_pressedButtons.ContainsKey(_panMouseButton))
            {
                return;
            }

            var mousePositionWhenButtonPressed = _pressedButtons[_panMouseButton];

            //Pan movement is in the Z - Y plane
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

            //Rotation is in the X - Y plane
            var vector = mousePositionWhenButtonPressed - _currentMousePosition;
            
            if(vector.Length != 0)
            {
                vector.Normalize();
            }

            vector = vector * 0.4;

            _mouseRotationY = -vector.Y;
            _mouseRotationZ = vector.X;
        }
    }
}