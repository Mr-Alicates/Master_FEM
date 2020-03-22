using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Commands;

namespace POC3D.ViewModel
{
    public class NewForceViewModel : Observable
    {
        private readonly ProblemViewModel _problemViewModel;
        private NodeViewModel _node;

        public NewForceViewModel(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;

            _problemViewModel.Forces.CollectionChanged += ForcesChanged;
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

        private void ForcesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(AvailableNodesForNewForces));
        }
    }
}