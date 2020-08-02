using System.Linq;
using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.ViewModel.Implementation;

namespace POC3D.ViewModel.Calculations
{
    public static class MatrixHelper
    {
        internal static bool CanProblemBeSolved(ProblemViewModel problemViewModel)
        {
            //To solve the problem, the 3 displacements must be constrained
            //In this kind of problem it means 3 non-collinear fixed nodes

            return problemViewModel.Nodes.Any(x => x.IsXFixed) &&
                problemViewModel.Nodes.Any(x => x.IsYFixed) &&
                problemViewModel.Nodes.Any(x => x.IsZFixed);
        }

        public static NumericMatrix BuildElementLocalStiffnessMatrix(ElementViewModel elementViewModel)
        {
            var element = elementViewModel.Element;

            return new NumericMatrix(2, 2)
            {
                [0, 0] = element.K,
                [0, 1] = -element.K,
                [1, 0] = -element.K,
                [1, 1] = element.K
            };
        }

        public static NumericMatrix BuildTransformationMatrix(ElementCalculationViewModel elementCalculationViewModel)
        {
            var result = new NumericMatrix(2, 6)
            {
                [0, 0] = elementCalculationViewModel.Cx,
                [0, 1] = elementCalculationViewModel.Cy,
                [0, 2] = elementCalculationViewModel.Cz,

                [1, 3] = elementCalculationViewModel.Cx,
                [1, 4] = elementCalculationViewModel.Cy,
                [1, 5] = elementCalculationViewModel.Cz
            };

            return result;
        }

        public static NumericMatrix BuildElementGlobalStiffnessMatrix(ElementCalculationViewModel elementCalculationViewModel)
        {
            var transformationMatrix = elementCalculationViewModel.TransformationMatrix;

            var transformationMatrixTransposed = elementCalculationViewModel.TransformationMatrixTransposed;

            var localStiffnessMatrix = elementCalculationViewModel.LocalStiffnessMatrix;

            return transformationMatrixTransposed * localStiffnessMatrix * transformationMatrix;
        }

        public static NumericMatrix BuildCompactedMatrix(ProblemViewModel problem)
        {
            var rawMatrix = BuildGlobalStiffnessMatrix(problem);

            var index = 0;

            foreach (var node in problem.Nodes)
            {
                if (node.IsXFixed)
                {
                    rawMatrix.RemoveColumn(index);
                    rawMatrix.RemoveRow(index);
                }
                else
                {
                    index++;
                }

                if (node.IsYFixed)
                {
                    rawMatrix.RemoveColumn(index);
                    rawMatrix.RemoveRow(index);
                }
                else
                {
                    index++;
                }

                if (node.IsZFixed)
                {
                    rawMatrix.RemoveColumn(index);
                    rawMatrix.RemoveRow(index);
                }
                else
                {
                    index++;
                }
            }

            return rawMatrix;
        }

        public static NumericMatrix BuildCompactedForcesVector(ProblemViewModel problem)
        {
            var result = new NumericMatrix(problem.Nodes.Count * 3, 1);

            var index = 0;

            foreach (var node in problem.Nodes)
            {
                var appliedForce = problem.Forces.FirstOrDefault(force => force.Node == node);

                if (node.IsXFixed)
                {
                    result.RemoveRow(index);
                }
                else
                {
                    result[index, 0] = appliedForce?.ApplicationVector.X ?? 0;
                    index++;
                }

                if (node.IsYFixed)
                {
                    result.RemoveRow(index);
                }
                else
                {
                    result[index, 0] = appliedForce?.ApplicationVector.Y ?? 0;
                    index++;
                }

                if (node.IsZFixed)
                {
                    result.RemoveRow(index);
                }
                else
                {
                    result[index, 0] = appliedForce?.ApplicationVector.Z ?? 0;
                    index++;
                }
            }

            return result;
        }

        public static NumericMatrix SolveForDisplacements(ProblemCalculationViewModel problemCalculationViewModel)
        {
            var compactedStiffnessMatrix = problemCalculationViewModel.CompactedMatrix;

            var compactedForcesVector = problemCalculationViewModel.CompactedForcesVector;

            var solution = compactedStiffnessMatrix.Solve(compactedForcesVector);

            return solution;
        }

        public static NumericMatrix BuildFullSolvedDisplacementsVector(ProblemViewModel problem)
        {
            var result = new NumericMatrix(problem.Nodes.Count * 3, 1);

            var solvedDisplacementsVector = problem.ProblemCalculationViewModel.SolvedDisplacementsVector;

            var index = 0;
            var solvedIndex = 0;

            foreach (var node in problem.Nodes)
            {
                if (node.IsXFixed)
                {
                    result[index, 0] = 0;
                }
                else
                {
                    result[index, 0] = solvedDisplacementsVector[solvedIndex, 0];
                    solvedIndex++;
                }

                index++;

                if (node.IsYFixed)
                {
                    result[index, 0] = 0;
                }
                else
                {
                    result[index, 0] = solvedDisplacementsVector[solvedIndex, 0];
                    solvedIndex++;
                }

                index++;

                if (node.IsZFixed)
                {
                    result[index, 0] = 0;
                }
                else
                {
                    result[index, 0] = solvedDisplacementsVector[solvedIndex, 0];
                    solvedIndex++;
                }

                index++;
            }

            return result;
        }

        public static NumericMatrix SolveForReactionForces(ProblemCalculationViewModel problemCalculationViewModel)
        {
            var globalStiffnessMatrix = problemCalculationViewModel.GlobalStiffnessMatrix;

            var fullDisplacementsVector = problemCalculationViewModel.FullSolvedDisplacementsVector;

            var result = globalStiffnessMatrix * fullDisplacementsVector;

            return result;
        }

        public static CorrespondenceMatrix BuildCorrespondenceMatrix(ProblemViewModel problem)
        {
            var result = new CorrespondenceMatrix();

            foreach (var node in problem.Nodes) result.AddNode(node);
            foreach (var element in problem.Elements) result.AddElement(element);

            return result;
        }

        public static NumericMatrix BuildGlobalStiffnessMatrix(ProblemViewModel problem)
        {
            var correspondenceMatrix = problem.ProblemCalculationViewModel.CorrespondenceMatrix;

            var nodeCount = correspondenceMatrix.NodeIndexes.Count;

            var unAssembledStiffnessMatrix = Enumerable.Range(0, nodeCount)
                .Select(x => new NumericMatrix[nodeCount])
                .ToArray();

            foreach (var element in problem.Elements)
            {
                var elementGlobalStiffnessMatrix = element.ElementCalculationViewModel.GlobalStiffnessMatrix;

                var originNodeIndex = correspondenceMatrix.NodeIndexes[element.Origin];
                var destinationNodeIndex = correspondenceMatrix.NodeIndexes[element.Destination];

                unAssembledStiffnessMatrix[originNodeIndex][originNodeIndex] +=
                    elementGlobalStiffnessMatrix.GetSubMatrix(0, 0, 3);
                unAssembledStiffnessMatrix[originNodeIndex][destinationNodeIndex] +=
                    elementGlobalStiffnessMatrix.GetSubMatrix(0, 3, 3);
                unAssembledStiffnessMatrix[destinationNodeIndex][originNodeIndex] +=
                    elementGlobalStiffnessMatrix.GetSubMatrix(3, 0, 3);
                unAssembledStiffnessMatrix[destinationNodeIndex][destinationNodeIndex] +=
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