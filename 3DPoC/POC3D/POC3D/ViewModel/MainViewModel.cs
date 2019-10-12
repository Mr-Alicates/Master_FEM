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
        private CameraViewModel _cameraViewModel;
        private BodyViewModel _bodyViewModel;

        public MainViewModel()
        {
            _cameraViewModel = new CameraViewModel();
            _bodyViewModel = new BodyViewModel();
        }

        public CameraViewModel Camera => _cameraViewModel;
        public BodyViewModel Body => _bodyViewModel;
    }    
}
