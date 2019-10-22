using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model
{
    public class ModelProblem
    {
        private readonly List<IModelElement> _elements = new List<IModelElement>();
        private readonly List<ModelNode> _nodes = new List<ModelNode>();

        public ModelProblem(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<IModelElement> Elements => _elements;

        public List<ModelNode> Nodes => _nodes;

        public ModelNode AddNode()
        {
            ModelNode result = ModelNode.CreateNewNode();
            _nodes.Add(result);
            return result;
        }

        public IModelElement AddBarElement(ModelNode node1, ModelNode node2)
        {
            if (!_nodes.Contains(node1))
            {
                throw new InvalidOperationException();
            }

            if (!_nodes.Contains(node2))
            {
                throw new InvalidOperationException();
            }

            var element = new ModelBarElement(node1, node2);

            _elements.Add(element);

            return element;
        }

        public void DeleteNode(ModelNode selectedNodeNode)
        {
            _nodes.Remove(selectedNodeNode);
        }
    }
}
