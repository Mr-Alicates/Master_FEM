using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace POC3D.ViewModel.Commands
{
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