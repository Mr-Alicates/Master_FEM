using POC3D.ViewModel.Implementation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace POC3D.ViewModel.Commands
{
    public class DeleteNodeCommand : ICommand
    {
        private readonly ProblemViewModel _problemViewModel;
        private bool _canExecute;

        public DeleteNodeCommand(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;

            problemViewModel.PropertyChanged += PropertiesChanged;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter)) _problemViewModel.DeleteSelectedNode();
        }

        private void PropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            _canExecute =
                _problemViewModel.SelectedNode != null &&
                _problemViewModel.Elements.All(element => element.Origin != _problemViewModel.SelectedNode && element.Destination != _problemViewModel.SelectedNode) &&
                _problemViewModel.Forces.All(force => force.Node != _problemViewModel.SelectedNode);
            
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}