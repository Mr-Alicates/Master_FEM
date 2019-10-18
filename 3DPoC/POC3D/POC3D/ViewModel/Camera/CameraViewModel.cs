using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Camera
{
    public class CameraViewModel : BaseCameraViewModel
    {
        private const int RotationDelta = 1;

        public Vector3D LookDirection
        {
            get
            {
                Vector3D vector = new Vector3D(1, 0, 0);

                Transform3DGroup transformGroup = new Transform3DGroup()
                {
                    Children = new Transform3DCollection()
                    {
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,1,0), RotationY)),
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,0,1), RotationZ)),
                    }
                };

                var transformedVector = transformGroup.Transform(vector);
                return transformedVector;
            }
        }

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


        #region Rotation

        public void RollUp()
        {
            RotationX = RotationX + RotationDelta;
        }

        public void RollDown()
        {
            RotationX = RotationX - RotationDelta;
        }

        public void YawUp()
        {
            RotationZ = RotationZ + RotationDelta;
        }

        public void YawDown()
        {
            RotationZ = RotationZ - RotationDelta;
        }
        
        public void PitchUp()
        {
            RotationY = RotationY + RotationDelta;
        }

        public void PitchDown()
        {
            RotationY = RotationY - RotationDelta;
        }

        #endregion

        #region Movement

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
