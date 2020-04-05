using POC3D.ViewModel.InterfaceManagement.CameraControl;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Tests.Helper
{
    public class FakeCameraViewModel : ICameraViewModel
    {
        public FakeCameraViewModel()
        {
            UnaryUp = new Vector3D(0, 0, 1);
            UnaryLeft = new Vector3D(0, 1, 0);
            UnaryForward = new Vector3D(1, 0, 0);
        }

        public Vector3D UnaryUp { get; set; }

        public Vector3D UnaryForward { get; set; }

        public Vector3D UnaryLeft { get; set; }

        public Point3D Position { get; set; }

        public Point3D LookAtPoint { get; set; }

        public Vector3D MoveVector { get; set; }

        public Point3D MovePoint { get; set; }

        public double Rotation => RotationX + RotationY + RotationZ;

        public double RotationX { get; set; }
        
        public double RotationY { get; set; }

        public double RotationZ { get; set; }

        public void LookAt(Point3D lookAtPoint)
        {
            LookAtPoint = lookAtPoint;
        }

        public void Move(Vector3D delta)
        {
            MoveVector = delta;
        }

        public void Move(Point3D newPosition)
        {
            MovePoint = newPosition;
        }

        public void Rotate(double rotationX, double rotationY, double rotationZ)
        {
            RotationX = rotationX;
            RotationY = rotationY;
            RotationZ = rotationZ;
        }
    }
}
