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
        private readonly List<ModelForce> _forces = new List<ModelForce>();

        public ModelProblem(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<IModelElement> Elements => _elements;

        public List<ModelNode> Nodes => _nodes;

        public List<ModelForce> Forces => _forces;

        public ModelNode AddNode()
        {
            ModelNode result = ModelNode.CreateNewNode();
            _nodes.Add(result);
            return result;
        }

        public ModelForce AddForce(ModelNode node)
        {
            if (!_nodes.Contains(node))
            {
                throw new InvalidOperationException();
            }

            var force = new ModelForce(node);

            _forces.Add(force);

            return force;
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

        public void DeleteElement(IModelElement element)
        {
            _elements.Remove(element);
        }

        public void DeleteForce(ModelForce force)
        {
            _forces.Remove(force);
        }
    }
}
