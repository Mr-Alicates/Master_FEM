using System;
using System.Collections.Generic;

namespace POC3D.Model
{
    public class ModelProblem : IModelProblem
    {
        private readonly List<IModelElement> _elements = new List<IModelElement>();

        public ModelProblem(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<IModelElement> Elements => _elements;

        public List<IModelNode> Nodes { get; } = new List<IModelNode>();

        public List<IModelForce> Forces { get; } = new List<IModelForce>();

        public IModelNode AddNode()
        {
            var result = ModelNode.CreateNewNode();
            Nodes.Add(result);
            return result;
        }

        public IModelForce AddForce(IModelNode node)
        {
            if (!Nodes.Contains(node)) throw new InvalidOperationException();

            var force = new ModelForce(node);

            Forces.Add(force);

            return force;
        }

        public IModelElement AddBarElement(IModelNode node1, IModelNode node2)
        {
            if (!Nodes.Contains(node1)) throw new InvalidOperationException();

            if (!Nodes.Contains(node2)) throw new InvalidOperationException();

            var element = new ModelBarElement(node1, node2);

            _elements.Add(element);

            return element;
        }

        public void DeleteNode(IModelNode selectedNodeNode)
        {
            Nodes.Remove(selectedNodeNode);
        }

        public void DeleteElement(IModelElement element)
        {
            _elements.Remove(element);
        }

        public void DeleteForce(IModelForce force)
        {
            Forces.Remove(force);
        }
    }
}