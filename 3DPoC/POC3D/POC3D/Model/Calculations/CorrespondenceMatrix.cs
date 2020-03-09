using System.Collections.Generic;
using POC3D.ViewModel;

namespace POC3D.Model.Calculations
{
    public class CorrespondenceMatrix
    {
        public readonly IDictionary<(int, int), List<ElementViewModel>> Matrix;

        public readonly IDictionary<NodeViewModel, int> NodeIndexes;

        public int Rows => NodeIndexes.Count;
        public int Columns => NodeIndexes.Count;

        public CorrespondenceMatrix()
        {
            NodeIndexes = new Dictionary<NodeViewModel, int>();
            Matrix = new Dictionary<(int, int), List<ElementViewModel>>();
        }

        public void AddNode(NodeViewModel node)
        {
            if (NodeIndexes.ContainsKey(node)) return;

            var newNodeIndex = NodeIndexes.Count;

            NodeIndexes.Add(node, newNodeIndex);

            for (var index = 0; index < newNodeIndex; index++)
            {
                Matrix.Add((index, newNodeIndex), new List<ElementViewModel>());
                Matrix.Add((newNodeIndex, index), new List<ElementViewModel>());
            }

            Matrix.Add((newNodeIndex, newNodeIndex), new List<ElementViewModel>());
        }

        public void AddElement(ElementViewModel element)
        {
            var originNodeIndex = NodeIndexes[element.Origin];
            var destinationNodeIndex = NodeIndexes[element.Destination];

            Matrix[(originNodeIndex, originNodeIndex)].Add(element);
            Matrix[(originNodeIndex, destinationNodeIndex)].Add(element);
            Matrix[(destinationNodeIndex, originNodeIndex)].Add(element);
            Matrix[(destinationNodeIndex, destinationNodeIndex)].Add(element);
        }
    }
}