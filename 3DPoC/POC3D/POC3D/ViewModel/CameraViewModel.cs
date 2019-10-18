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
            UpDirection = new Vector3D(0, 0, 1);
            _lookDirectionX = 180;
            _lookDirectionY = 0;
        }

        public EventHandler OnCameraViewModelChanged;

        private Point3D _position;
        private double _lookDirectionX;
        private double _lookDirectionY;

        public Point3D Position
        {
            get => _position;
            set
            {
                _position = value;
                OnCameraViewModelChanged.Invoke(this, null);
            }
        }

        public Vector3D UpDirection { get; }

        //Z is always the same as the position because we dont have any pitch for now
        public Vector3D LookDirection => new Vector3D(_lookDirectionX, _lookDirectionY, 0);

        public void RotateLeft()
        {
            int deltaX = 0;
            int deltaY = 0;

            if (_lookDirectionX >= 0)
            {
                if (_lookDirectionY == 180)
                {
                    // 2nd quarter
                    deltaX = -1;
                    deltaY = -1;
                }
                else if (_lookDirectionY >= 0)
                {
                    // 1st quarter
                    deltaX = -1;
                    deltaY = +1;
                }
                else
                {
                    // 4th quarter
                    deltaX = +1;
                    deltaY = +1;
                }
            }
            else
            {
                if (_lookDirectionY > 0)
                {
                    //2nd quarter
                    deltaX = -1;
                    deltaY = -1;
                }
                else
                {
                    //3rd quarter
                    deltaX = +1;
                    deltaY = -1;
                }
            }

            _lookDirectionX += deltaX;
            _lookDirectionY += deltaY;
            OnCameraViewModelChanged.Invoke(this, null);
        }

        public void RotateRight()
        {
            int deltaX = 0;
            int deltaY = 0;

            if (_lookDirectionX >= 0)
            {
                if (_lookDirectionY == -180)
                {
                    // 3rd quarter
                    deltaX = -1;
                    deltaY = +1;
                }
                else if (_lookDirectionY >= 0)
                {
                    // 1st quarter
                    deltaX = +1;
                    deltaY = -1;
                }
                else
                {
                    // 4th quarter
                    deltaX = -1;
                    deltaY = -1;
                }
            }
            else
            {
                if (_lookDirectionY > 0)
                {
                    //2nd quarter
                    deltaX = +1;
                    deltaY = +1;
                }
                else
                {
                    //3rd quarter
                    deltaX = -1;
                    deltaY = +1;
                }
            }

            _lookDirectionX += deltaX;
            _lookDirectionY += deltaY;
            OnCameraViewModelChanged.Invoke(this, null);
        }

        #region Movement

        private Vector3D _unaryUp => new Vector3D(0, 0, 1);
        
        private Vector3D _unaryLeft
        {
            get
            {
                var left = new Vector3D(-_lookDirectionY, _lookDirectionX, 0);
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
