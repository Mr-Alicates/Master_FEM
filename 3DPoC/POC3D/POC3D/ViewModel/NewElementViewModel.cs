using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POC3D.ViewModel
{
    public class NewElementViewModel : Observable
    {
        private readonly ProblemViewModel _problemViewModel;
        private NodeViewModel _originNode;
        private NodeViewModel _destinationNode;

        public NewElementViewModel(ProblemViewModel problemViewModel)
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
    }

    public class AddElementCommand : ICommand
    {
        private readonly NewElementViewModel _newElementViewModel;
        private readonly ProblemViewModel _problemViewModel;

        public event EventHandler CanExecuteChanged;
        private bool _canExecute;

        public AddElementCommand(NewElementViewModel newElementViewModel, ProblemViewModel problemViewModel)
        {
            _newElementViewModel = newElementViewModel;
            _problemViewModel = problemViewModel;
            _newElementViewModel.PropertyChanged += PropertiesChanged;
        }

        private void PropertiesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _canExecute = _newElementViewModel.OriginNode != null &&
                _newElementViewModel.DestinationNode != null &&
                _newElementViewModel.OriginNode.Name != _newElementViewModel.DestinationNode.Name;
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _problemViewModel.AddBarElement(_newElementViewModel.OriginNode, _newElementViewModel.DestinationNode);
        }
    }
}
