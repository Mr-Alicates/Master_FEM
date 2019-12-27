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

        private Visibility _elementAddingControlVisibility = Visibility.Collapsed;
        private Visibility _elementDetailsControlVisibility = Visibility.Collapsed;
        private Visibility _elementListingControlVisibility = Visibility.Collapsed;

        private Visibility _forceAddingControlVisibility = Visibility.Collapsed;
        private Visibility _forceDetailsControlVisibility = Visibility.Collapsed;
        private Visibility _forceListingControlVisibility = Visibility.Collapsed;

        private Point _lastMousePosition;

        private Visibility _nodeDetailsControlVisibility = Visibility.Collapsed;
        private Visibility _nodeListingControlVisibility = Visibility.Collapsed;

        public InterfaceControlViewModel(ProblemViewModel problemViewModel, CameraViewModel cameraViewModel)
        {
            _problemViewModel = problemViewModel;
            _cameraViewModel = cameraViewModel;

            problemViewModel.PropertyChanged += ProblemViewModel_PropertyChanged;
        }

        public ICommand HideAllCommand => new ButtonCommand(HideAllControls);

        private void HideAllControls()
        {
            ElementAddingControlVisibility = Visibility.Collapsed;
            ElementDetailsControlVisibility = Visibility.Collapsed;
            ElementListingControlVisibility = Visibility.Collapsed;

            NodeDetailsControlVisibility = Visibility.Collapsed;
            NodeListingControlVisibility = Visibility.Collapsed;

            ForceAddingControlVisibility = Visibility.Collapsed;
            ForceDetailsControlVisibility = Visibility.Collapsed;
            ForceListingControlVisibility = Visibility.Collapsed;
        }

        #region Nodes

        public ICommand ShowSelectedNodeDetailsCommand => new ButtonCommand(ShowSelectedNodeDetails);

        public ICommand AddNodeCommand => _problemViewModel.AddNodeCommand;

        public ICommand DeleteNodeCommand => new ButtonCommand(DeleteNode);

        public ICommand ShowNodeListingCommand => new ButtonCommand(ShowNodeListing);

        private void ShowSelectedNodeDetails()
        {
            HideAllControls();

            NodeDetailsControlVisibility = Visibility.Visible;
        }

        private void DeleteNode()
        {
            HideAllControls();

            _problemViewModel.DeleteNodeCommand.Execute(null);
        }

        private void ShowNodeListing()
        {
            HideAllControls();

            NodeDetailsControlVisibility = Visibility.Visible;
            NodeListingControlVisibility = Visibility.Visible;
        }

        #endregion

        #region Elements

        public ICommand ShowSelectedElementDetailsCommand => new ButtonCommand(ShowSelectedElementDetails);

        public ICommand AddElementCommand => new ButtonCommand(AddElement);

        public ICommand DeleteElementCommand => new ButtonCommand(DeleteElement);

        public ICommand ShowElementListingCommand => new ButtonCommand(ShowElementListing);

        private void ShowSelectedElementDetails()
        {
            HideAllControls();

            ElementDetailsControlVisibility = Visibility.Visible;
        }

        private void AddElement()
        {
            HideAllControls();

            ElementAddingControlVisibility = Visibility.Visible;
        }

        private void DeleteElement()
        {
            HideAllControls();

            _problemViewModel.DeleteElementCommand.Execute(null);
        }

        private void ShowElementListing()
        {
            HideAllControls();

            ElementDetailsControlVisibility = Visibility.Visible;
            ElementListingControlVisibility = Visibility.Visible;
        }

        #endregion

        #region Forces

        public ICommand ShowSelectedForceDetailsCommand => new ButtonCommand(ShowSelectedForceDetails);

        public ICommand AddForceCommand => new ButtonCommand(AddForce);

        public ICommand DeleteForceCommand => new ButtonCommand(DeleteForce);

        public ICommand ShowForceListingCommand => new ButtonCommand(ShowForceListing);

        private void ShowSelectedForceDetails()
        {
            HideAllControls();

            ForceDetailsControlVisibility = Visibility.Visible;
        }

        private void AddForce()
        {
            HideAllControls();

            ForceAddingControlVisibility = Visibility.Visible;
        }

        private void DeleteForce()
        {
            HideAllControls();

            _problemViewModel.DeleteForceCommand.Execute(null);
        }

        private void ShowForceListing()
        {
            HideAllControls();

            ForceDetailsControlVisibility = Visibility.Visible;
            ForceListingControlVisibility = Visibility.Visible;
        }

        #endregion

        #region Viewport

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

        #endregion

        #region ControlsVisibility

        private void ProblemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_problemViewModel.SelectedNode) &&
                _problemViewModel.SelectedNode != null)
                ShowSelectedNodeDetailsCommand.Execute(null);

            if (e.PropertyName == nameof(_problemViewModel.SelectedElement) &&
                _problemViewModel.SelectedElement != null)
                ShowSelectedElementDetailsCommand.Execute(null);

            if (e.PropertyName == nameof(_problemViewModel.SelectedForce) &&
                _problemViewModel.SelectedForce != null)
                ShowSelectedForceDetailsCommand.Execute(null);
        }

        public Visibility NodeDetailsControlVisibility
        {
            get => _nodeDetailsControlVisibility;
            private set
            {
                _nodeDetailsControlVisibility = value;
                OnPropertyChanged(nameof(NodeDetailsControlVisibility));
            }
        }

        public Visibility ElementDetailsControlVisibility
        {
            get => _elementDetailsControlVisibility;
            private set
            {
                _elementDetailsControlVisibility = value;
                OnPropertyChanged(nameof(ElementDetailsControlVisibility));
            }
        }

        public Visibility ForceDetailsControlVisibility
        {
            get => _forceDetailsControlVisibility;
            private set
            {
                _forceDetailsControlVisibility = value;
                OnPropertyChanged(nameof(ForceDetailsControlVisibility));
            }
        }

        public Visibility NodeListingControlVisibility
        {
            get => _nodeListingControlVisibility;
            private set
            {
                _nodeListingControlVisibility = value;
                OnPropertyChanged(nameof(NodeListingControlVisibility));
            }
        }

        public Visibility ElementListingControlVisibility
        {
            get => _elementListingControlVisibility;
            private set
            {
                _elementListingControlVisibility = value;
                OnPropertyChanged(nameof(ElementListingControlVisibility));
            }
        }

        public Visibility ForceListingControlVisibility
        {
            get => _forceListingControlVisibility;
            private set
            {
                _forceListingControlVisibility = value;
                OnPropertyChanged(nameof(ForceListingControlVisibility));
            }
        }

        public Visibility ElementAddingControlVisibility
        {
            get => _elementAddingControlVisibility;
            private set
            {
                _elementAddingControlVisibility = value;
                OnPropertyChanged(nameof(ElementAddingControlVisibility));
            }
        }

        public Visibility ForceAddingControlVisibility
        {
            get => _forceAddingControlVisibility;
            private set
            {
                _forceAddingControlVisibility = value;
                OnPropertyChanged(nameof(ForceAddingControlVisibility));
            }
        }

        #endregion
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