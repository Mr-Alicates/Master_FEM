using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

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

    public class AddForceCommand : ICommand
    {
        private readonly NewForceViewModel _newForceViewModel;
        private readonly ProblemViewModel _problemViewModel;
        private bool _canExecute;

        public AddForceCommand(NewForceViewModel newForceViewModel, ProblemViewModel problemViewModel)
        {
            _newForceViewModel = newForceViewModel;
            _problemViewModel = problemViewModel;
            _newForceViewModel.PropertyChanged += PropertiesChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _problemViewModel.AddForce(_newForceViewModel.Node);

            _newForceViewModel.Node = null;
        }

        private void PropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            //I don't want two forces on the same node
            _canExecute = _newForceViewModel.Node != null &&
                          _problemViewModel.Forces.All(existingForce => existingForce.Node != _newForceViewModel.Node);

            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}