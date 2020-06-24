using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Commands;
using POC3D.ViewModel.Implementation;

namespace POC3D.ViewModel.InterfaceManagement
{
    public class ForceManagementViewModel : Observable
    {
        private readonly ProblemViewModel _problemViewModel;
        private NodeViewModel _node;

        public ForceManagementViewModel(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;

            _problemViewModel.Forces.CollectionChanged += ProblemChanged;
            _problemViewModel.Nodes.CollectionChanged += ProblemChanged;
        }

        public NodeViewModel Node
        {
            get => _node;
            set
            {
                _node = value;
                OnPropertyChanged(nameof(Node));
            }
        }

        public IEnumerable<NodeViewModel> AvailableNodesForNewForces => _problemViewModel.Nodes.Except(_problemViewModel.Forces.Select(force => force.Node));

        public ICommand AddForceCommand => new AddForceCommand(this, _problemViewModel);

        public ICommand DeleteForceCommand => new DeleteForceCommand(_problemViewModel);

        private void ProblemChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(AvailableNodesForNewForces));
        }
    }
}