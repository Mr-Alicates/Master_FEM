using POC3D.ViewModel.Implementation;
using POC3D.ViewModel.InterfaceManagement;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace POC3D.ViewModel.Commands
{
    public class AddForceCommand : ICommand
    {
        private readonly ForceManagementViewModel _newForceViewModel;
        private readonly ProblemViewModel _problemViewModel;
        private bool _canExecute;

        public AddForceCommand(ForceManagementViewModel newForceViewModel, ProblemViewModel problemViewModel)
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