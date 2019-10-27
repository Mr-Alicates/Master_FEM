using POC3D.ViewModel.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class MainViewModel : Observable
    {
        public MainViewModel()
        {
            CameraViewModel = new CameraViewModel();
            ProblemViewModel = new ProblemViewModel();
            CameraControlViewModel = new VisualControlViewModel(CameraViewModel);
        }

        public CameraViewModel CameraViewModel { get; }

        public ProblemViewModel ProblemViewModel { get; }

        public VisualControlViewModel CameraControlViewModel { get; }
    }    
}
