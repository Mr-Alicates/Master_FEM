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

        public static Matrix BuildElementLocalStiffnessMatrix(IModelElement element)
        {
            return new Matrix(6, 6)
            {
                [0,0] = element.K,
                [0,3]= -element.K,
                [3, 0] = -element.K,
                [3,3] = element.K
            };
        }

        public static Matrix BuildTransformationMatrix(IModelElement element)
        {
            var alpha = element.LocalCoordinateSystemRotationAngles.Alpha;
            var beta = element.LocalCoordinateSystemRotationAngles.Beta;
            
            var result = new Matrix(6, 6)
            {
                [0, 0] = Math.Cos(beta),
                [0, 1] = Math.Sin(-alpha) * Math.Sin(beta),
                [0, 2] = Math.Cos(-alpha) * Math.Sin(beta),

                [1, 1] = Math.Cos(-alpha),
                [1, 2] = -Math.Sin(-alpha),

                [2, 0] = -Math.Sin(beta),
                [2, 1] = Math.Cos(beta) * Math.Sin(-alpha),
                [2, 2] = Math.Cos(-alpha) * Math.Cos(beta),

                [3, 3] = Math.Cos(beta),
                [3, 4] = Math.Sin(-alpha) * Math.Sin(beta),
                [3, 5] = Math.Cos(-alpha) * Math.Sin(beta),

                [4, 4] = Math.Cos(-alpha),
                [4, 5] = -Math.Sin(-alpha),

                [5, 3] = -Math.Sin(beta),
                [5, 4] = Math.Cos(beta) * Math.Sin(-alpha),
                [5, 5] = Math.Cos(-alpha) * Math.Cos(beta),
            };

            return result;
        }

        public static Matrix BuildElementGlobalStiffnessMatrix(IModelElement element)
        {
            var transformationMatrix = element.TransformationMatrix;

            var transformationMatrixTransposed = transformationMatrix.Transpose();

            var localStiffnessMatrix = element.LocalStiffnessMatrix;

            return transformationMatrixTransposed * localStiffnessMatrix * transformationMatrixTransposed;
        }
    }
}