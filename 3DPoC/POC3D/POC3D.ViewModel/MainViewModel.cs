using POC3D.ViewModel.Base;
using POC3D.ViewModel.Implementation;
using POC3D.ViewModel.InterfaceManagement;

namespace POC3D.ViewModel
{
    public class MainViewModel : Observable
    {
        public MainViewModel()
        {
            CameraViewModel = new CameraViewModel();
            ProblemViewModel = new ProblemViewModel();
            InterfaceControlViewModel = new InterfaceControlViewModel(ProblemViewModel, CameraViewModel);
        }

        public CameraViewModel CameraViewModel { get; }

        public ProblemViewModel ProblemViewModel { get; }

        public InterfaceControlViewModel InterfaceControlViewModel { get; }
    }
}