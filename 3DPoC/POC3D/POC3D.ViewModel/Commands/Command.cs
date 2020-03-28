using System;
using System.Windows.Input;

namespace POC3D.ViewModel.Commands
{
    public class Command : ICommand
    {
        private readonly Action _buttonAction;

        public Command(Action buttonAction)
        {
            _buttonAction = buttonAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _buttonAction();
        }

        public event EventHandler CanExecuteChanged;
    }
}