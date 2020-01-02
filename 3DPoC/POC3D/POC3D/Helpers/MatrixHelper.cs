using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.Model.Calculations;

namespace POC3D.Helpers
{
    public static class MatrixHelper
    {
        public static RotationAngles CalculateRotationAnglesForElement(IModelElement element)
        {
            var absoluteX = new Vector3D(1, 0, 0);
            var absoluteY = new Vector3D(0, 1, 0);

            var localX = new Vector3D(element.Direction.X, element.Direction.Y, element.Direction.Z);

            var localY = Vector3D.CrossProduct(localX, absoluteX);

            if (localY.Length == 0)
                //both vectors are aligned so angles are zero;
                return new RotationAngles(0, 0);

            localX.Normalize();
            localY.Normalize();

            var angleBetweenXVectors = Vector3D.AngleBetween(localX, absoluteX) % 180;
            var angleBetweenYVectors = Vector3D.AngleBetween(localY, absoluteY) % 180;

            if (double.IsNaN(angleBetweenXVectors)) angleBetweenXVectors = 0;

            if (double.IsNaN(angleBetweenYVectors)) angleBetweenYVectors = 0;

            //var rotationAroundX = new RotateTransform3D(new AxisAngleRotation3D(absoluteX, -angleBetweenYVectors));
            //var rotationAroundY = new RotateTransform3D(new AxisAngleRotation3D(absoluteY, angleBetweenXVectors));

            return new RotationAngles(-angleBetweenYVectors, angleBetweenXVectors);
        }
    }
}