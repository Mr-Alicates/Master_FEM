namespace POC3D.ViewModel.InterfaceManagement.CameraControl
{
    public abstract class BaseCameraControlViewModel
    {
        protected ICameraViewModel CameraViewModel;

        protected BaseCameraControlViewModel(ICameraViewModel cameraViewModel)
        {
            CameraViewModel = cameraViewModel;
        }

        public abstract void UpdateCamera();
    }
}
