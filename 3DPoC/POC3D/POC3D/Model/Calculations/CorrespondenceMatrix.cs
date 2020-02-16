using System.Collections.Generic;

namespace POC3D.Model.Calculations
{
    public class CorrespondenceMatrix
    {
        public readonly IDictionary<(int, int), List<IModelElement>> Matrix;

        public readonly IDictionary<ModelNode, int> NodeIndexes;

        public CorrespondenceMatrix()
        {
            NodeIndexes = new Dictionary<ModelNode, int>();
            Matrix = new Dictionary<(int, int), List<IModelElement>>();
        }

        public void AddNode(ModelNode node)
        {
            if (NodeIndexes.ContainsKey(node)) return;

            var newNodeIndex = NodeIndexes.Count;

            NodeIndexes.Add(node, newNodeIndex);

            for (var index = 0; index < newNodeIndex; index++)
            {
                Matrix.Add((index, newNodeIndex), new List<IModelElement>());
                Matrix.Add((newNodeIndex, index), new List<IModelElement>());
            }

            Matrix.Add((newNodeIndex, newNodeIndex), new List<IModelElement>());
        }

        public void AddElement(IModelElement element)
        {
            var originNodeIndex = NodeIndexes[element.OriginNode];
            var destinationNodeIndex = NodeIndexes[element.DestinationNode];

            Matrix[(originNodeIndex, originNodeIndex)].Add(element);
            Matrix[(originNodeIndex, destinationNodeIndex)].Add(element);
            Matrix[(destinationNodeIndex, originNodeIndex)].Add(element);
            Matrix[(destinationNodeIndex, destinationNodeIndex)].Add(element);
        }
    }
}