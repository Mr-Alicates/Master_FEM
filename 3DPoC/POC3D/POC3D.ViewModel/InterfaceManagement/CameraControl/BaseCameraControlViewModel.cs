namespace POC3D.ViewModel.InterfaceManagement.CameraControl
{
    public abstract class BaseCameraControlViewModel
    {
        protected CameraViewModel CameraViewModel;

        protected BaseCameraControlViewModel(CameraViewModel cameraViewModel)
        {
            CameraViewModel = cameraViewModel;
        }

        public abstract void UpdateCamera();
    }
}
