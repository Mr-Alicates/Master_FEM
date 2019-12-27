using System;
using System.ComponentModel;
using System.Windows.Input;

namespace POC3D.ViewModel.Commands
{
    public class DeleteForceCommand : ICommand
    {
        private readonly ProblemViewModel _problemViewModel;
        private bool _canExecute;

        public DeleteForceCommand(ProblemViewModel problemViewModel)
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
            if (CanExecute(parameter)) _problemViewModel.DeleteSelectedForce();
        }

        private void PropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            _canExecute = _problemViewModel.SelectedForce != null;
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}