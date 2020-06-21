using POC3D.ViewModel.Implementation;
using System;
using System.Windows.Input;

namespace POC3D.ViewModel.Commands
{
    public class AddMaterialCommand : ICommand
    {
        private readonly ProblemViewModel _problemViewModel;

        public AddMaterialCommand(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _problemViewModel.AddMaterial();
        }
    }
}