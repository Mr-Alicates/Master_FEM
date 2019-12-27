using System;
using System.ComponentModel;
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
            _canExecute = _problemViewModel.SelectedNode != null;
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}