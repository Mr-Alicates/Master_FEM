using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace POC3D.ViewModel
{
    public class NewElementViewModel : Observable
    {
        private readonly ProblemViewModel _problemViewModel;
        private NodeViewModel _destinationNode;
        private NodeViewModel _originNode;

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
        private bool _canExecute;

        public AddElementCommand(NewElementViewModel newElementViewModel, ProblemViewModel problemViewModel)
        {
            _newElementViewModel = newElementViewModel;
            _problemViewModel = problemViewModel;
            _newElementViewModel.PropertyChanged += PropertiesChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _problemViewModel.AddBarElement(_newElementViewModel.OriginNode, _newElementViewModel.DestinationNode);

            _newElementViewModel.OriginNode = null;
            _newElementViewModel.DestinationNode = null;
        }

        private void PropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            _canExecute = _newElementViewModel.OriginNode != null &&
                          _newElementViewModel.DestinationNode != null &&
                          _newElementViewModel.OriginNode.Name != _newElementViewModel.DestinationNode.Name &&
                          _problemViewModel.Elements.All(existingElement =>
                              existingElement.Origin != _newElementViewModel.OriginNode ||
                              existingElement.Destination != _newElementViewModel.DestinationNode) &&
                          _problemViewModel.Elements.All(existingElement =>
                              existingElement.Origin != _newElementViewModel.DestinationNode ||
                              existingElement.Destination != _newElementViewModel.OriginNode);
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}