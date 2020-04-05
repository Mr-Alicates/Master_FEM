using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.InterfaceManagement.CameraControl
{
    public interface ICameraViewModel
    {
        Vector3D UnaryUp { get; }

        Vector3D UnaryForward { get; }

        Vector3D UnaryLeft { get; }

        Point3D Position { get; }

        void Move(Vector3D delta);

        void Move(Point3D newPosition);

        void Rotate(double rotationX, double rotationY, double rotationZ);

        void LookAt(Point3D lookAtPoint);
    }
}
