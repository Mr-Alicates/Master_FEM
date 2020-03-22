using System.Windows.Input;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Commands;

namespace POC3D.ViewModel
{
    public class ElementManagementViewModel : Observable
    {
        private readonly ProblemViewModel _problemViewModel;
        private NodeViewModel _destinationNode;
        private NodeViewModel _originNode;

        public ElementManagementViewModel(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;
        }

        public NodeViewModel OriginNode
        {
            get => _originNode;
            set
            {
                _originNode = value;
                OnPropertyChanged(nameof(OriginNode));
            }
        }

        public NodeViewModel DestinationNode
        {
            get => _destinationNode;
            set
            {
                _destinationNode = value;
                OnPropertyChanged(nameof(DestinationNode));
            }
        }

        public ICommand AddElementCommand => new AddElementCommand(this, _problemViewModel);

        public ICommand DeleteElementCommand => new DeleteElementCommand(_problemViewModel);
    }
}