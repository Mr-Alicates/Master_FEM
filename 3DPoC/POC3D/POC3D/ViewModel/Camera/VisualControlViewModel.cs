using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POC3D.ViewModel.Camera
{
    public class VisualControlViewModel : Observable
    {
        private readonly CameraViewModel _cameraViewModel;
        private Point _lastMousePosition;

        public VisualControlViewModel(CameraViewModel cameraViewModel)
        {
            _cameraViewModel = cameraViewModel;
        }
                
        public void ReactToMouseWheelMovement(int delta)
        {
            int movements = Math.Abs(delta / 10);

            for (int i = 0; i < movements; i++)
            {
                if (delta > 0)
                {
                    _cameraViewModel.MoveForward();
                }

                if (delta < 0)
                {
                    _cameraViewModel.MoveBackwards();
                }
            }
        }

        public void ReactToMouseMovement(
            MouseButtonState middleButton, 
            MouseButtonState rightButton, 
            Point currentCursorPosition)
        {
            if (middleButton == MouseButtonState.Pressed)
            {
                ReactToCameraPanMouse(currentCursorPosition);
            }

            if (rightButton == MouseButtonState.Pressed)
            {
                ReactToCameraRotateMouse(currentCursorPosition);
            }

            _lastMousePosition = currentCursorPosition;
        }

        public void ReactToKeyBoardKeyDown(bool isLeftShiftPressed, Key pressedKey)
        {
            if (isLeftShiftPressed)
            {
                ReactToCameraRotationKeyDown(pressedKey);
            }
            else
            {
                ReactToMovementKeyDown(pressedKey);
            }
        }

        private void ReactToCameraRotateMouse(Point currentCursorPosition)
        {
            if (currentCursorPosition.X < _lastMousePosition.X)
            {
                _cameraViewModel.CameraRotationZUp();
            }

            if (currentCursorPosition.X > _lastMousePosition.X)
            {
                _cameraViewModel.CameraRotationZDown();
            }

            if (currentCursorPosition.Y < _lastMousePosition.Y)
            {
                _cameraViewModel.CameraRotationYDown();
            }

            if (currentCursorPosition.Y > _lastMousePosition.Y)
            {
                _cameraViewModel.CameraRotationYUp();
            }
        }
        
        private void ReactToCameraPanMouse(Point currentCursorPosition)
        {
            if (currentCursorPosition.X < _lastMousePosition.X)
            {
                _cameraViewModel.MoveLeft();
            }

            if (currentCursorPosition.X > _lastMousePosition.X)
            {
                _cameraViewModel.MoveRight();
            }

            if (currentCursorPosition.Y < _lastMousePosition.Y)
            {
                _cameraViewModel.MoveUp();
            }

            if (currentCursorPosition.Y > _lastMousePosition.Y)
            {
                _cameraViewModel.MoveDown();
            }
        }

        private void ReactToCameraRotationKeyDown(Key pressedKey)
        {
            switch (pressedKey)
            {
                case Key.A:
                    _cameraViewModel.CameraRotationZUp();
                    break;
                case Key.D:
                    _cameraViewModel.CameraRotationZDown();
                    break;
                case Key.S:
                    _cameraViewModel.CameraRotationYUp();
                    break;
                case Key.W:
                    _cameraViewModel.CameraRotationYDown();
                    break;
            }
        }

        private void ReactToMovementKeyDown(Key pressedKey)
        {
            switch (pressedKey)
            {
                case Key.A:
                    _cameraViewModel.MoveLeft();
                    break;
                case Key.D:
                    _cameraViewModel.MoveRight();
                    break;
                case Key.W:
                    _cameraViewModel.MoveForward();
                    break;
                case Key.S:
                    _cameraViewModel.MoveBackwards();
                    break;
                case Key.R:
                    _cameraViewModel.MoveUp();
                    break;
                case Key.F:
                    _cameraViewModel.MoveDown();
                    break;
            }
        }
    }
}
