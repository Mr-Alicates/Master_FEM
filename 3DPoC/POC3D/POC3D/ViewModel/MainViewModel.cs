using POC3D.ViewModel.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Camera = new CameraViewModel();
            Problem = new ProblemViewModel();
            CameraControlViewModel = new CameraControlViewModel(Camera);
        }

        public CameraViewModel Camera { get; }

        public ProblemViewModel Problem { get; }

        public CameraControlViewModel CameraControlViewModel { get; }
    }    
}
