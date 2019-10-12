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
            upDirection = new Vector3D(0, 0, 1);
            lookDirectionX = 10;
            lookDirectionY = 0;
        }

        public EventHandler OnCameraViewModelChanged;

        private Point3D position;
        private Vector3D upDirection;
        private double lookDirectionX;
        private double lookDirectionY;

        public Point3D Position
        {
            get => position;
            set
            {
                position = value;
                OnCameraViewModelChanged.Invoke(this, null);
            }
        }

        public Vector3D UpDirection => upDirection;

        //Z is always the same as the position because we dont have any pitch for now
        public Vector3D LookDirection => new Vector3D(lookDirectionX, lookDirectionY, 0);

        public void RotateCounterClockwise()
        {
            int deltaX = 0;
            int deltaY = 0;

            if (lookDirectionX >= 0)
            {
                if (lookDirectionY == 10)
                {
                    // 2nd quarter
                    deltaX = -1;
                    deltaY = -1;
                }
                else if (lookDirectionY >= 0)
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
                if (lookDirectionY > 0)
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

            lookDirectionX += deltaX;
            lookDirectionY += deltaY;
            OnCameraViewModelChanged.Invoke(this, null);
        }

        public void RotateClockwise()
        {
            int deltaX = 0;
            int deltaY = 0;

            if (lookDirectionX >= 0)
            {
                if (lookDirectionY == -10)
                {
                    // 3rd quarter
                    deltaX = -1;
                    deltaY = +1;
                }
                else if (lookDirectionY >= 0)
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
                if (lookDirectionY > 0)
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

            lookDirectionX += deltaX;
            lookDirectionY += deltaY;
            OnCameraViewModelChanged.Invoke(this, null);
        }

        #region Movement

        private Vector3D _unaryUp => new Vector3D(0, 0, 1);

        private Vector3D _unaryLeft => new Vector3D(-lookDirectionY, lookDirectionX, 0);

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
