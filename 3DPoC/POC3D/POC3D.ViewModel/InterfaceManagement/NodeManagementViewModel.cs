using POC3D.ViewModel.Commands;
using POC3D.ViewModel.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POC3D.ViewModel.InterfaceManagement
{
    public class NodeManagementViewModel
    {
        private readonly ProblemViewModel _problemViewModel;

        public NodeManagementViewModel(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;
        }

        public ICommand AddNodeCommand => new AddNodeCommand(_problemViewModel);

        public ICommand DeleteNodeCommand => new DeleteNodeCommand(_problemViewModel);
    }
}
