using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Commands;
using POC3D.ViewModel.Implementation;
using POC3D.ViewModel.InterfaceManagement.CameraControl;

namespace POC3D.ViewModel.InterfaceManagement
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

        private Visibility _nodeDetailsControlVisibility = Visibility.Collapsed;
        private Visibility _nodeListingControlVisibility = Visibility.Collapsed;

        private Visibility _materialDetailsControlVisibility = Visibility.Collapsed;
        private Visibility _materialListingControlVisibility = Visibility.Collapsed;

        private Visibility _configurationControlVisibility = Visibility.Collapsed;

        public InterfaceControlViewModel(ProblemViewModel problemViewModel, CameraViewModel cameraViewModel)
        {
            _problemViewModel = problemViewModel;
            _cameraViewModel = cameraViewModel;

            ElementManagementViewModel = new ElementManagementViewModel(_problemViewModel);
            ForceManagementViewModel = new ForceManagementViewModel(_problemViewModel);
            NodeManagementViewModel = new NodeManagementViewModel(_problemViewModel);
            MaterialManagementViewModel = new MaterialManagementViewModel(_problemViewModel);

            KeyboardCameraControlViewModel = new KeyboardCameraControlViewModel(_cameraViewModel);
            MouseCameraControlViewModel = new MouseCameraControlViewModel(_cameraViewModel);

            problemViewModel.PropertyChanged += ProblemViewModel_PropertyChanged;

            Application.Current.Dispatcher.InvokeAsync(UpdateCamera);
        }

        public ElementManagementViewModel ElementManagementViewModel { get; }

        public ForceManagementViewModel ForceManagementViewModel { get; }

        public NodeManagementViewModel NodeManagementViewModel { get; }

        public MaterialManagementViewModel MaterialManagementViewModel { get; }

        public ICommand HideAllCommand => new Command(HideAllControls);

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

            MaterialDetailsControlVisibility = Visibility.Collapsed;
            MaterialListingControlVisibility = Visibility.Collapsed;

            ConfigurationControlVisibility = Visibility.Collapsed;
        }

        #region Nodes

        public ICommand ShowSelectedNodeDetailsCommand => new Command(ShowSelectedNodeDetails);

        public ICommand AddNodeCommand => NodeManagementViewModel.AddNodeCommand;

        public ICommand ShowNodeListingCommand => new Command(ShowNodeListing);

        private void ShowSelectedNodeDetails()
        {
            ElementAddingControlVisibility = Visibility.Collapsed;
            ElementDetailsControlVisibility = Visibility.Collapsed;
            ElementListingControlVisibility = Visibility.Collapsed;

            ForceAddingControlVisibility = Visibility.Collapsed;
            ForceDetailsControlVisibility = Visibility.Collapsed;
            ForceListingControlVisibility = Visibility.Collapsed;

            NodeDetailsControlVisibility = Visibility.Visible;

            MaterialDetailsControlVisibility = Visibility.Collapsed;
            MaterialListingControlVisibility = Visibility.Collapsed;

            ConfigurationControlVisibility = Visibility.Collapsed;
        }

        private void ShowNodeListing()
        {
            HideAllControls();

            NodeDetailsControlVisibility = Visibility.Visible;
            NodeListingControlVisibility = Visibility.Visible;
        }

        #endregion

        #region Elements

        public ICommand ShowSelectedElementDetailsCommand => new Command(ShowSelectedElementDetails);

        public ICommand AddElementCommand => new Command(AddElement);

        public ICommand ShowElementListingCommand => new Command(ShowElementListing);

        private void ShowSelectedElementDetails()
        {
            ElementAddingControlVisibility = Visibility.Collapsed;

            NodeDetailsControlVisibility = Visibility.Collapsed;
            NodeListingControlVisibility = Visibility.Collapsed;

            ForceAddingControlVisibility = Visibility.Collapsed;
            ForceDetailsControlVisibility = Visibility.Collapsed;
            ForceListingControlVisibility = Visibility.Collapsed;

            ElementDetailsControlVisibility = Visibility.Visible;

            MaterialDetailsControlVisibility = Visibility.Collapsed;
            MaterialListingControlVisibility = Visibility.Collapsed;

            ConfigurationControlVisibility = Visibility.Collapsed;
        }

        private void AddElement()
        {
            HideAllControls();

            ElementAddingControlVisibility = Visibility.Visible;
        }

        private void ShowElementListing()
        {
            HideAllControls();

            ElementDetailsControlVisibility = Visibility.Visible;
            ElementListingControlVisibility = Visibility.Visible;
        }

        #endregion

        #region Forces

        public ICommand ShowSelectedForceDetailsCommand => new Command(ShowSelectedForceDetails);

        public ICommand AddForceCommand => new Command(AddForce);

        public ICommand ShowForceListingCommand => new Command(ShowForceListing);

        private void ShowSelectedForceDetails()
        {
            ElementAddingControlVisibility = Visibility.Collapsed;
            ElementDetailsControlVisibility = Visibility.Collapsed;
            ElementListingControlVisibility = Visibility.Collapsed;

            NodeDetailsControlVisibility = Visibility.Collapsed;
            NodeListingControlVisibility = Visibility.Collapsed;

            ForceAddingControlVisibility = Visibility.Collapsed;

            ForceDetailsControlVisibility = Visibility.Visible;

            MaterialDetailsControlVisibility = Visibility.Collapsed;
            MaterialListingControlVisibility = Visibility.Collapsed;

            ConfigurationControlVisibility = Visibility.Collapsed;
        }

        private void AddForce()
        {
            HideAllControls();

            ForceAddingControlVisibility = Visibility.Visible;
        }

        private void ShowForceListing()
        {
            HideAllControls();

            ForceDetailsControlVisibility = Visibility.Visible;
            ForceListingControlVisibility = Visibility.Visible;
        }

        #endregion

        #region Materials

        public ICommand AddMaterialCommand => MaterialManagementViewModel.AddMaterialCommand;

        public ICommand ShowMaterialListingCommand => new Command(ShowMaterialListing);

        private void ShowMaterialListing()
        {
            HideAllControls();

            MaterialDetailsControlVisibility = Visibility.Visible;
            MaterialListingControlVisibility = Visibility.Visible;
        }

        #endregion

        #region Configuration

        public ICommand ShowConfigurationCommand => new Command(ShowConfiguration);

        private void ShowConfiguration()
        {
            HideAllControls();

            ConfigurationControlVisibility = Visibility.Visible;
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

        public Visibility MaterialListingControlVisibility
        {
            get => _materialListingControlVisibility;
            private set
            {
                _materialListingControlVisibility = value;
                OnPropertyChanged(nameof(MaterialListingControlVisibility));
            }
        }

        public Visibility MaterialDetailsControlVisibility
        {
            get => _materialDetailsControlVisibility;
            private set
            {
                _materialDetailsControlVisibility = value;
                OnPropertyChanged(nameof(MaterialDetailsControlVisibility));
            }
        }

        public Visibility ConfigurationControlVisibility
        {
            get => _configurationControlVisibility;
            private set
            {
                _configurationControlVisibility = value;
                OnPropertyChanged(nameof(ConfigurationControlVisibility));
            }
        }

        #endregion

        #region Camera

        private async Task UpdateCamera()
        {
            while (true)
            {
                KeyboardCameraControlViewModel.UpdateCamera();
                MouseCameraControlViewModel.UpdateCamera();

                await Task.Delay(1);
            }
        }

        public KeyboardCameraControlViewModel KeyboardCameraControlViewModel { get; }

        public MouseCameraControlViewModel MouseCameraControlViewModel { get; }

        #endregion
    }
}