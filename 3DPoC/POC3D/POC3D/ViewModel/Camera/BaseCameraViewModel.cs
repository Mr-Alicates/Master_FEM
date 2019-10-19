using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Camera
{
    public class BaseCameraViewModel
    {
        public EventHandler OnCameraViewModelChanged;

        private Vector3D _upDirection;
        private Point3D _position;

        private int _environmentRotationX;
        private int _environmentRotationY;
        private int _environmentRotationZ;

        private int _cameraRotationY;
        private int _cameraRotationZ;

        public BaseCameraViewModel()
        {
            _upDirection = new Vector3D(0, 0, 1);
        }

        public Vector3D UpDirection => _upDirection;

        public int CameraRotationY
        {
            get => _cameraRotationY;
            protected set
            {
                _cameraRotationY = value;
                OnCameraViewModelChanged.Invoke(this, null);
            }
        }

        public int CameraRotationZ
        {
            get => _cameraRotationZ;
            protected set
            {
                _cameraRotationZ = value;
                OnCameraViewModelChanged.Invoke(this, null);
            }
        }

        public Point3D Position
        {
            get => _position;
            set
            {
                _position = value;
                OnCameraViewModelChanged.Invoke(this, null);
            }
        }
    }
}
