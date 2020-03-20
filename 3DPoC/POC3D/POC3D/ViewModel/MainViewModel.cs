using POC3D.ViewModel.Base;

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