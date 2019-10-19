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
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,1,0), CameraRotationY)),
                        new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0,0,1), CameraRotationZ)),
                    }
                };

                var transformedVector = transformGroup.Transform(vector);
                return transformedVector;
            }
        }

        public Vector3D UnaryUp
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

                var up = transformGroup.Transform(UnaryForward);
                up.Normalize();
                return up;
            }
        }

        public Vector3D UnaryLeft
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

                var left = transformGroup.Transform(UnaryForward);
                left.Normalize();
                return left;
            }
        }

        public Vector3D UnaryForward
        {
            get
            {
                var forward = LookDirection;
                forward.Normalize();
                return forward;
            }
        }

        #region CameraRotation

        public void CameraRotationZUp()
        {
            CameraRotationZ = CameraRotationZ + RotationDelta;
        }

        public void CameraRotationZDown()
        {
            CameraRotationZ = CameraRotationZ - RotationDelta;
        }
        
        public void CameraRotationYUp()
        {
            CameraRotationY = CameraRotationY + RotationDelta;
        }

        public void CameraRotationYDown()
        {
            CameraRotationY = CameraRotationY - RotationDelta;
        }

        #endregion

        #region Movement

        public void MoveForward()
        {
            Position = Position + UnaryForward;
        }

        public void MoveBackwards()
        {
            Position = Position - UnaryForward;
        }

        public void MoveUp()
        {
            Position = Position + UnaryUp;
        }

        public void MoveDown()
        {
            Position = Position - UnaryUp;
        }

        public void MoveLeft()
        {
            Position = Position + UnaryLeft;
        }

        public void MoveRight()
        {
            Position = Position - UnaryLeft;
        }

        #endregion
    }
}
