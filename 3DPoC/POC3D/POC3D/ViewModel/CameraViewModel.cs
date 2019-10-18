using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class CameraViewModel
    {
        public CameraViewModel()
        {
            _upDirection = new Vector3D(0, 0, 1);
        }

        public EventHandler OnCameraViewModelChanged;

        private Vector3D _upDirection;
        private Point3D _position;
        private int _cameraRotation;
        private int _rotationY;
        private int _rotationZ;


        public int CameraRotation
        {
            get => _cameraRotation;
            set
            {
                _cameraRotation = value;
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

        public Vector3D UpDirection => _upDirection;

        public Vector3D LookDirection
        {
            get
            {
                Vector3D vector = new Vector3D(1, 0, 0);

                Transform3DGroup transformGroup = new Transform3DGroup()
                {
                    Children = new Transform3DCollection()
                    {
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,1,0), _rotationY)),
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,0,1), _rotationZ)),
                    }
                };

                var transformedVector = transformGroup.Transform(vector);
                _upDirection.Normalize();

                return transformedVector;
            }
        }

        public void RollUp()
        {
            CameraRotation = CameraRotation + 5;
            OnCameraViewModelChanged.Invoke(null, null);
        }

        public void RollDown()
        {
            CameraRotation = CameraRotation - 5;
            OnCameraViewModelChanged.Invoke(null, null);
        }

        public void YawUp()
        {
            _rotationZ = _rotationZ + 5;
            OnCameraViewModelChanged.Invoke(null, null);
        }

        public void YawDown()
        {
            _rotationZ = _rotationZ - 5;
            OnCameraViewModelChanged.Invoke(null, null);
        }
        
        public void PitchUp()
        {
            _rotationY = _rotationY + 5;
            OnCameraViewModelChanged.Invoke(null, null);
        }

        public void PitchDown()
        {
            _rotationY = _rotationY - 5;
            OnCameraViewModelChanged.Invoke(null, null);
        }

        #region Movement

        private Vector3D _unaryUp
        {
            get
            {
                Transform3DGroup transformGroup = new Transform3DGroup()
                {
                    Children = new Transform3DCollection()
                    {
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,1,0), -90)),
                    }
                };

                var up = transformGroup.Transform(_unaryForward);
                up.Normalize();
                return up;
            }
        }

        private Vector3D _unaryLeft
        {
            get
            {
                Transform3DGroup transformGroup = new Transform3DGroup()
                {
                    Children = new Transform3DCollection()
                    {
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,0,1), 90)),
                    }
                };

                var left = transformGroup.Transform(_unaryForward);
                left.Normalize();
                return left;
            }
        }

        private Vector3D _unaryForward
        {
            get
            {
                var forward = LookDirection;
                forward.Normalize();
                return forward;
            }
        }

        public void MoveForward()
        {
            Position = Position + _unaryForward;
        }

        public void MoveBackwards()
        {
            Position = Position - _unaryForward;
        }

        public void MoveUp()
        {
            Position = Position + _unaryUp;
        }

        public void MoveDown()
        {
            Position = Position - _unaryUp;
        }

        public void MoveLeft()
        {
            Position = Position + _unaryLeft;
        }

        public void MoveRight()
        {
            Position = Position - _unaryLeft;
        }

        #endregion
    }
}
