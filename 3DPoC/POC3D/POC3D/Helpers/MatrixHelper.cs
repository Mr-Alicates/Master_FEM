using System;
using System.Linq;
using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.Model.Calculations;

namespace POC3D.Helpers
{
    public static class MatrixHelper
    {
        internal static bool CanProblemBeSolved(ModelProblem modelProblem)
        {
            //To solve the problem, the 3 displacements must be constrained
            //In this kind of problem it means 3 non-collinear fixed nodes

            var fixedNodes = modelProblem.Nodes
                .Where(x => x.IsFixed)
                .Select(x=>x.Coordinates)
                .ToList();

            if(fixedNodes.Count < 3)
            {
                return false;
            }

            var firstNode = fixedNodes.First();

            var restOfNodes = fixedNodes.Skip(1);

            var vectors = restOfNodes
                .Select(x => new ModelVector(firstNode, x))
                .Select(vector => new Vector3D(vector.X, vector.Y, vector.Z))
                .ToList();

            var firstVector = vectors.First();

            var restOfVectors = vectors.Skip(1);

            var angles = restOfVectors
                .Select(vector => Vector3D.AngleBetween(firstVector, vector))
                .Where(angle => angle != 0)
                .ToList();

            return angles.Any();
            
        }

        public static NumericMatrix BuildElementLocalStiffnessMatrix(IModelElement element)
        {
            return new NumericMatrix(2, 2)
            {
                [0, 0] = element.K,
                [0, 1] = -element.K,
                [1, 0] = -element.K,
                [1, 1] = element.K
            };
        }

        public static NumericMatrix BuildTransformationMatrix(IModelElement element)
        {
            var result = new NumericMatrix(2, 6)
            {
                [0, 0] = element.Cx,
                [0, 1] = element.Cy,
                [0, 2] = element.Cz,

                [1, 3] = element.Cx,
                [1, 4] = element.Cy,
                [1, 5] = element.Cz,
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

        public static NumericMatrix BuildCompactedMatrix(ModelProblem problem)
        {
            var rawMatrix = BuildGlobalStiffnessMatrix(problem);
            
            int index = 0;

            foreach(var node in problem.Nodes)
            {
                if (node.IsFixed)
                {
                    rawMatrix.RemoveColumn(index);
                    rawMatrix.RemoveColumn(index);
                    rawMatrix.RemoveColumn(index);

                    rawMatrix.RemoveRow(index);
                    rawMatrix.RemoveRow(index);
                    rawMatrix.RemoveRow(index);
                }
                else
                {
                    index = index + 3;
                }
            }

            return rawMatrix;
        }

        public static NumericMatrix BuildCompactedForcesVector(ModelProblem problem)
        {
            var result = new NumericMatrix(problem.Nodes.Count * 3, 1);

            int index = 0;

            foreach (var node in problem.Nodes)
            {
                if (node.IsFixed)
                {
                    result.RemoveRow(index);
                    result.RemoveRow(index);
                    result.RemoveRow(index);
                }
                else
                {
                    var appliedForce = problem.Forces.FirstOrDefault(force => force.Node == node);

                    result[index, 0] = appliedForce?.ApplicationVector.X ?? 0;
                    result[index + 1, 0] = appliedForce?.ApplicationVector.Y ?? 0;
                    result[index + 2, 0] = appliedForce?.ApplicationVector.Z ?? 0;

                    index = index + 3;
                }
            }

            return result;
        }

        public static NumericMatrix SolveForDisplacements(ModelProblem problem)
        {
            var compactedStiffnessMatrix = problem.CompactedMatrix;

            var compactedForcesVector = problem.CompactedForcesVector;

            var solution = compactedStiffnessMatrix.Solve(compactedForcesVector);

            return solution;
        }

        public static CorrespondenceMatrix BuildCorrespondenceMatrix(ModelProblem problem)
        {
            var result = new CorrespondenceMatrix();

            foreach (var node in problem.Nodes) result.AddNode(node);
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

                unAssembledStiffnessMatrix[originNodeIndex][originNodeIndex] += elementGlobalStiffnessMatrix.GetSubMatrix(0, 0, 3);
                unAssembledStiffnessMatrix[originNodeIndex][destinationNodeIndex] += elementGlobalStiffnessMatrix.GetSubMatrix(0, 3, 3);
                unAssembledStiffnessMatrix[destinationNodeIndex][originNodeIndex] += elementGlobalStiffnessMatrix.GetSubMatrix(3, 0, 3);
                unAssembledStiffnessMatrix[destinationNodeIndex][destinationNodeIndex] += elementGlobalStiffnessMatrix.GetSubMatrix(3, 3, 3);
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