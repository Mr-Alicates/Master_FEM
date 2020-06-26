using POC3D.ViewModel.Configuration;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.InterfaceManagement.CameraControl
{
    public class KeyboardCameraControlViewModel : BaseCameraControlViewModel
    {
        private HashSet<Key> _pressedKeys = new HashSet<Key>();

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

        public KeyboardCameraControlViewModel(ICameraViewModel cameraViewModel)
            : base(cameraViewModel)
        {
        }

        public void ReactToKeyboardKeyDown(Key pressedKey)
        {
            _pressedKeys.Add(pressedKey);
        }

        public void ReactToKeyboardKeyUp(Key liftedKey)
        {
            _pressedKeys.Remove(liftedKey);
        }

        public override void UpdateCamera()
        {
            if (_pressedKeys.Contains(_special))
            {
                DoKeyboardRotation();
            }
            else
            {
                DoKeyboardMovement();
            }
        }

        private void DoKeyboardMovement()
        {
            var delta = new Vector3D();

            if (_pressedKeys.Contains(_forward))
            {
                delta += CameraViewModel.UnaryForward;
            }
            if (_pressedKeys.Contains(_backward))
            {
                delta -= CameraViewModel.UnaryForward;
            }
            if (_pressedKeys.Contains(_left))
            {
                delta += CameraViewModel.UnaryLeft;
            }
            if (_pressedKeys.Contains(_right))
            {
                delta -= CameraViewModel.UnaryLeft;
            }
            if (_pressedKeys.Contains(_up))
            {
                delta += CameraViewModel.UnaryUp;
            }
            if (_pressedKeys.Contains(_down))
            {
                delta -= CameraViewModel.UnaryUp;
            }

            CameraViewModel.Move(delta);
        }

        private void DoKeyboardRotation()
        {
            var keyboardRotationX = 0.0;
            var keyboardRotationY = 0.0;
            var keyboardRotationZ = 0.0;

            if (_pressedKeys.Contains(_pitchUp))
            {
                keyboardRotationY = ApplicationConfiguration.KeyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_pitchDown))
            {
                keyboardRotationY = -ApplicationConfiguration.KeyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_yawUp))
            {
                keyboardRotationZ = ApplicationConfiguration.KeyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_yawDown))
            {
                keyboardRotationZ = -ApplicationConfiguration.KeyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_rollUp))
            {
                keyboardRotationX = ApplicationConfiguration.KeyboardRotationDelta;
            }
            if (_pressedKeys.Contains(_rollDown))
            {
                keyboardRotationX = -ApplicationConfiguration.KeyboardRotationDelta;
            }

            CameraViewModel.Rotate(keyboardRotationX, keyboardRotationY, keyboardRotationZ);
        }
    }
}
