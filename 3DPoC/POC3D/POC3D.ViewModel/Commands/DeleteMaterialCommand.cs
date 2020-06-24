using POC3D.ViewModel.Implementation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace POC3D.ViewModel.Commands
{
    public class DeleteMaterialCommand : ICommand
    {
        private readonly ProblemViewModel _problemViewModel;
        private bool _canExecute;

        public DeleteMaterialCommand(ProblemViewModel problemViewModel)
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
            if (CanExecute(parameter)) _problemViewModel.DeleteSelectedMaterial();
        }

        private void PropertiesChanged(object sender, PropertyChangedEventArgs e)
        {
            _canExecute = _problemViewModel.SelectedMaterial != null && _problemViewModel.Elements.All(element => element.Material != _problemViewModel.SelectedMaterial);
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}