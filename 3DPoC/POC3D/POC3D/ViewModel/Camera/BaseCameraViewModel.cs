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
        private int _rotationX;
        private int _rotationY;
        private int _rotationZ;

        public BaseCameraViewModel()
        {
            _upDirection = new Vector3D(0, 0, 1);
        }

        public Vector3D UpDirection => _upDirection;

        public int RotationX
        {
            get => _rotationX;
            set
            {
                _rotationX = value;
                OnCameraViewModelChanged.Invoke(this, null);
            }
        }

        public int RotationY
        {
            get => _rotationY;
            set
            {
                _rotationY = value;
                OnCameraViewModelChanged.Invoke(this, null);
            }
        }

        public int RotationZ
        {
            get => _rotationZ;
            set
            {
                _rotationZ = value;
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
