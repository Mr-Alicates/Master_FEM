using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace POC3D.ViewModel
{
    public class InterfaceControlViewModel : Observable
    {
        private readonly CameraViewModel _cameraViewModel;
        private readonly ProblemViewModel _problemViewModel;
        private Visibility _elementsControlVisibility = Visibility.Collapsed;
        private Visibility _forcesControlVisibility = Visibility.Collapsed;
        private Point _lastMousePosition;
        private Visibility _nodesControlVisibility = Visibility.Collapsed;

        public InterfaceControlViewModel(ProblemViewModel problemViewModel, CameraViewModel cameraViewModel)
        {
            _problemViewModel = problemViewModel;
            _cameraViewModel = cameraViewModel;

            problemViewModel.PropertyChanged += ProblemViewModel_PropertyChanged;
        }

        public Visibility NodesControlVisibility
        {
            get => _nodesControlVisibility;
            private set
            {
                _nodesControlVisibility = value;
                OnPropertyChanged(nameof(NodesControlVisibility));
            }
        }

        public Visibility ElementsControlVisibility
        {
            get => _elementsControlVisibility;
            private set
            {
                _elementsControlVisibility = value;
                OnPropertyChanged(nameof(ElementsControlVisibility));
            }
        }

        public Visibility ForcesControlVisibility
        {
            get => _forcesControlVisibility;
            private set
            {
                _forcesControlVisibility = value;
                OnPropertyChanged(nameof(ForcesControlVisibility));
            }
        }

        public ICommand ShowNodesControlCommand => new ButtonCommand(ShowNodeControls);

        public ICommand ShowElementsControlCommand => new ButtonCommand(ShowElementControls);

        public ICommand ShowForcesControlConmmand => new ButtonCommand(ShowForcesControls);

        private void ProblemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_problemViewModel.SelectedNode) &&
                _problemViewModel.SelectedNode != null)
            {
                NodesControlVisibility = Visibility.Visible;
                ElementsControlVisibility = Visibility.Collapsed;
                ForcesControlVisibility = Visibility.Collapsed;
            }

            if (e.PropertyName == nameof(_problemViewModel.SelectedElement) &&
                _problemViewModel.SelectedElement != null)
            {
                NodesControlVisibility = Visibility.Collapsed;
                ElementsControlVisibility = Visibility.Visible;
                ForcesControlVisibility = Visibility.Collapsed;
            }

            if (e.PropertyName == nameof(_problemViewModel.SelectedForce) &&
                _problemViewModel.SelectedForce != null)
            {
                NodesControlVisibility = Visibility.Collapsed;
                ElementsControlVisibility = Visibility.Collapsed;
                ForcesControlVisibility = Visibility.Visible;
            }
        }

        public void ShowNodeControls()
        {
            NodesControlVisibility = Visibility.Visible;
            ElementsControlVisibility = Visibility.Collapsed;
            ForcesControlVisibility = Visibility.Collapsed;
        }

        public void ShowElementControls()
        {
            NodesControlVisibility = Visibility.Collapsed;
            ElementsControlVisibility = Visibility.Visible;
            ForcesControlVisibility = Visibility.Collapsed;
        }

        public void ShowForcesControls()
        {
            NodesControlVisibility = Visibility.Collapsed;
            ElementsControlVisibility = Visibility.Collapsed;
            ForcesControlVisibility = Visibility.Visible;
        }

        public void ReactToMouseWheelMovement(int delta)
        {
            var movements = Math.Abs(delta / 10);

            for (var i = 0; i < movements; i++)
            {
                if (delta > 0) _cameraViewModel.MoveForward();

                if (delta < 0) _cameraViewModel.MoveBackwards();
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
            if (currentCursorPosition.X < _lastMousePosition.X) _cameraViewModel.CameraRotationZUp();

            if (currentCursorPosition.X > _lastMousePosition.X) _cameraViewModel.CameraRotationZDown();

            if (currentCursorPosition.Y < _lastMousePosition.Y) _cameraViewModel.CameraRotationYDown();

            if (currentCursorPosition.Y > _lastMousePosition.Y) _cameraViewModel.CameraRotationYUp();
        }

        private void ReactToCameraPanMouse(Point currentCursorPosition)
        {
            if (currentCursorPosition.X < _lastMousePosition.X) _cameraViewModel.MoveLeft();

            if (currentCursorPosition.X > _lastMousePosition.X) _cameraViewModel.MoveRight();

            if (currentCursorPosition.Y < _lastMousePosition.Y) _cameraViewModel.MoveUp();

            if (currentCursorPosition.Y > _lastMousePosition.Y) _cameraViewModel.MoveDown();
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

    public class ButtonCommand : ICommand
    {
        private readonly Action _buttonAction;

        public ButtonCommand(Action buttonAction)
        {
            _buttonAction = buttonAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _buttonAction();
        }

        public event EventHandler CanExecuteChanged;
    }
}