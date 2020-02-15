using System;
using System.Linq;
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

        internal static bool CanProblemBeSolved(ModelProblem modelProblem)
        {
            var globalStiffnessMatrix = BuildGlobalStiffnessMatrix(modelProblem);

            //A linear equation system with a singular matrix is not solvable because it has infinite solutions
            //A linear equation system matrix is singular when its determinant is zero
            return globalStiffnessMatrix.CalculateDeterminant() != 0;
        }

        public static NumericMatrix BuildElementLocalStiffnessMatrix(IModelElement element)
        {
            return new NumericMatrix(6, 6)
            {
                [0, 0] = element.K,
                [0, 3] = -element.K,
                [3, 0] = -element.K,
                [3, 3] = element.K
            };
        }

        public static NumericMatrix BuildTransformationMatrix(IModelElement element)
        {
            var alpha = element.LocalCoordinateSystemRotationAngles.Alpha;
            var beta = element.LocalCoordinateSystemRotationAngles.Beta;

            var result = new NumericMatrix(6, 6)
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
                [5, 5] = Math.Cos(-alpha) * Math.Cos(beta)
            };

            return result;
        }

        public static NumericMatrix BuildElementGlobalStiffnessMatrix(IModelElement element)
        {
            var transformationMatrix = element.TransformationMatrix;

            var transformationMatrixTransposed = transformationMatrix.Transpose();

            var localStiffnessMatrix = element.LocalStiffnessMatrix;

            return transformationMatrixTransposed * localStiffnessMatrix * transformationMatrix;
        }

        public static CorrespondenceMatrix BuildCorrespondenceMatrix(ModelProblem problem)
        {
            var result = new CorrespondenceMatrix();

            foreach (var element in problem.Elements) result.AddElement(element);

            return result;
        }

        public static NumericMatrix BuildGlobalStiffnessMatrix(ModelProblem problem)
        {
            var correspondenceMatrix = problem.CorrespondenceMatrix;

            var nodeCount = correspondenceMatrix.NodeIndexes.Count;

            var unAssembledStiffnessMatrix = Enumerable.Range(0, nodeCount)
                .Select(x => new NumericMatrix[nodeCount])
                .ToArray();

            foreach (var element in problem.Elements)
            {
                var elementGlobalStiffnessMatrix = element.GlobalStiffnessMatrix;

                var originNodeIndex = correspondenceMatrix.NodeIndexes[element.OriginNode];
                var destinationNodeIndex = correspondenceMatrix.NodeIndexes[element.DestinationNode];

                unAssembledStiffnessMatrix[originNodeIndex][originNodeIndex] =
                    elementGlobalStiffnessMatrix.GetSubMatrix(0, 0, 3);
                unAssembledStiffnessMatrix[originNodeIndex][destinationNodeIndex] =
                    elementGlobalStiffnessMatrix.GetSubMatrix(0, 3, 3);
                unAssembledStiffnessMatrix[destinationNodeIndex][originNodeIndex] =
                    elementGlobalStiffnessMatrix.GetSubMatrix(3, 0, 3);
                unAssembledStiffnessMatrix[destinationNodeIndex][destinationNodeIndex] =
                    elementGlobalStiffnessMatrix.GetSubMatrix(3, 3, 3);
            }

            var result = new NumericMatrix(nodeCount * 3, nodeCount * 3);

            for (var row = 0; row < nodeCount; row++)
            for (var column = 0; column < nodeCount; column++)
            {
                var sourceMatrix = unAssembledStiffnessMatrix[row][column];

                if (sourceMatrix != null) result.SetSubMatrix(row * 3, column * 3, sourceMatrix);
            }

            return result;
        }
    }
}