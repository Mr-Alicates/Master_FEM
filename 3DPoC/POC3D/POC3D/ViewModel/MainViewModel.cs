namespace POC3D.ViewModel
{
    public class MainViewModel : Observable
    {
        public MainViewModel()
        {
            CameraViewModel = new CameraViewModel();
            ProblemViewModel = new ProblemViewModel();
            InterfaceControlViewModel = new InterfaceControlViewModel(ProblemViewModel, CameraViewModel);

            ProblemViewModel.PropertyChanged += ProblemViewModel_PropertyChanged;
        }

        private void ProblemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ProblemViewModel));
        }

        public CameraViewModel CameraViewModel { get; }

        public ProblemViewModel ProblemViewModel { get; }

        public InterfaceControlViewModel InterfaceControlViewModel { get; }
    }
}