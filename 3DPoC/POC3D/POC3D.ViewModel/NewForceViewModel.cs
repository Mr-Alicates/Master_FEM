using System.Windows.Input;
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

        public ICommand AddForceCommand => new AddForceCommand(this, _problemViewModel);
    }
}