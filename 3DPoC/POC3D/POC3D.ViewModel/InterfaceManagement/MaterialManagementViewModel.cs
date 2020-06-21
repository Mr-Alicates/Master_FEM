using System.Windows.Input;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Commands;
using POC3D.ViewModel.Implementation;

namespace POC3D.ViewModel.InterfaceManagement
{
    public class MaterialManagementViewModel : Observable
    {
        private readonly ProblemViewModel _problemViewModel;

        public MaterialManagementViewModel(ProblemViewModel problemViewModel)
        {
            _problemViewModel = problemViewModel;
        }

        public ICommand AddMaterialCommand => new AddMaterialCommand(_problemViewModel);

        public ICommand DeleteMaterialCommand => new DeleteMaterialCommand(_problemViewModel);
    }
}