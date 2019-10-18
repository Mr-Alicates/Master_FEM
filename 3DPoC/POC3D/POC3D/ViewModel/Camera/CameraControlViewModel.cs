using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POC3D.ViewModel.Camera
{
    public class CameraControlViewModel
    {
        private CameraViewModel _cameraViewModel;

        private Point _clickedPoint;
        private MouseButton? _pressedButton;
        private DateTime _lastButtonPress;

        private TimeSpan GetTimeSinceLastMovement => DateTime.Now - _lastButtonPress;

        public CameraControlViewModel(CameraViewModel cameraViewModel)
        {
            _cameraViewModel = cameraViewModel;
        }

        public void ReactToMouseButtonDown(MouseButton pressedButton, Point currentCursorPosition)
        {
            _clickedPoint = currentCursorPosition;
            _pressedButton = pressedButton;
        }

        public void ReactToMouseWheelMovement(int delta)
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

        public void ReactToMouseMovement(MouseButtonState middleButton, MouseButtonState rightButton, Point currentCursorPosition)
        {
            if (GetTimeSinceLastMovement.TotalMilliseconds < 50)
            {
                return;
            }

            _lastButtonPress = DateTime.Now;

            if (_pressedButton == MouseButton.Middle)
            {
                if (middleButton == MouseButtonState.Released)
                {
                    _pressedButton = null;
                    return;
                }
                
                if (currentCursorPosition.X < _clickedPoint.X)
                {
                    _cameraViewModel.YawUp();
                }

                if (currentCursorPosition.X > _clickedPoint.X)
                {
                    _cameraViewModel.YawDown();
                }

                if (currentCursorPosition.Y < _clickedPoint.Y)
                {
                    _cameraViewModel.PitchDown();
                }

                if (currentCursorPosition.Y > _clickedPoint.Y)
                {
                    _cameraViewModel.PitchUp();
                }
            }

            if (_pressedButton == MouseButton.Right)
            {
                if (rightButton == MouseButtonState.Released)
                {
                    _pressedButton = null;
                    return;
                }

                if (currentCursorPosition.X < _clickedPoint.X)
                {
                    _cameraViewModel.MoveLeft();
                }

                if (currentCursorPosition.X > _clickedPoint.X)
                {
                    _cameraViewModel.MoveRight();
                }

                if (currentCursorPosition.Y < _clickedPoint.Y)
                {
                    _cameraViewModel.MoveUp();
                }

                if (currentCursorPosition.Y > _clickedPoint.Y)
                {
                    _cameraViewModel.MoveDown();
                }
            }
        }

        public void ReactToKeyBoardKeyDown(bool isLeftShiftPressed, Key pressedKey)
        {
            if (isLeftShiftPressed)
            {
                switch (pressedKey)
                {
                    case Key.A:
                        _cameraViewModel.YawUp();
                        break;
                    case Key.D:
                        _cameraViewModel.YawDown();
                        break;
                    case Key.S:
                        _cameraViewModel.PitchUp();
                        break;
                    case Key.W:
                        _cameraViewModel.PitchDown();
                        break;
                    case Key.E:
                        _cameraViewModel.RollUp();
                        break;
                    case Key.Q:
                        _cameraViewModel.RollDown();
                        break;
                }
            }
            else
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
}
