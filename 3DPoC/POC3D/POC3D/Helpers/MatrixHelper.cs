using System;
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

        public static Matrix6 BuildElementLocalStiffnessMatrix(IModelElement element)
        {
            var k = element.K;

            var rawMatrix = new double[][]
            {
                new double[] { k, 0, 0, -k, 0, 0},
                new double[] { 0, 0, 0, 0, 0, 0},
                new double[] { 0, 0, 0, 0, 0, 0},
                new double[] { -k, 0, 0, k, 0, 0},
                new double[] { 0, 0, 0, 0, 0, 0},
                new double[] { 0, 0, 0, 0, 0, 0} 
            };

            return new Matrix6(rawMatrix);
        }

        public static Matrix6 BuildTransformationMatrix(IModelElement element)
        {
            var alpha = element.LocalCoordinateSystemRotationAngles.Alpha;
            var beta = element.LocalCoordinateSystemRotationAngles.Beta;

            var rawMatrix = new[]
            {
                new[] {Math.Cos(beta), Math.Sin(-alpha) * Math.Sin(beta), Math.Cos(-alpha) * Math.Sin(beta), 0, 0, 0},
                new[] {0, Math.Cos(-alpha), -Math.Sin(-alpha), 0, 0, 0},
                new[] {-Math.Sin(beta), Math.Cos(beta) * Math.Sin(-alpha), Math.Cos(-alpha) * Math.Cos(beta), 0, 0, 0},

                new[] {0, 0, 0, Math.Cos(beta), Math.Sin(-alpha) * Math.Sin(beta), Math.Cos(-alpha) * Math.Sin(beta)},
                new[] {0, 0, 0, 0, Math.Cos(-alpha), -Math.Sin(-alpha)},
                new[] {0, 0, 0, -Math.Sin(beta), Math.Cos(beta) * Math.Sin(-alpha), Math.Cos(-alpha) * Math.Cos(beta)}
            };

            return new Matrix6(rawMatrix);
        }

        public static Matrix6 BuildElementGlobalStiffnessMatrix(IModelElement element)
        {
            var transformationMatrix = element.TransformationMatrix;

            var transformationMatrixTransposed = transformationMatrix.Transpose();

            var localStiffnessMatrix = element.LocalStiffnessMatrix;

            return transformationMatrixTransposed * localStiffnessMatrix * transformationMatrixTransposed;
        }
    }
}