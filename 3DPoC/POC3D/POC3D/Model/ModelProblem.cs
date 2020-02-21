using System;
using System.Collections.Generic;
using POC3D.Helpers;
using POC3D.Model.Calculations;

namespace POC3D.Model
{
    public class ModelProblem
    {
        private readonly List<IModelElement> _elements = new List<IModelElement>();

        public ModelProblem(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<IModelElement> Elements => _elements;

        public List<ModelNode> Nodes { get; } = new List<ModelNode>();

        public List<ModelForce> Forces { get; } = new List<ModelForce>();

        public ModelNode AddNode()
        {
            var result = ModelNode.CreateNewNode();
            Nodes.Add(result);
            return result;
        }

        public ModelForce AddForce(ModelNode node)
        {
            if (!Nodes.Contains(node)) throw new InvalidOperationException();

            var force = new ModelForce(node);

            Forces.Add(force);

            return force;
        }

        public IModelElement AddBarElement(ModelNode node1, ModelNode node2)
        {
            if (!Nodes.Contains(node1)) throw new InvalidOperationException();

            if (!Nodes.Contains(node2)) throw new InvalidOperationException();

            var element = new ModelBarElement(node1, node2);

            _elements.Add(element);

            return element;
        }

        public void DeleteNode(ModelNode selectedNodeNode)
        {
            Nodes.Remove(selectedNodeNode);
        }

        public void DeleteElement(IModelElement element)
        {
            _elements.Remove(element);
        }

        public void DeleteForce(ModelForce force)
        {
            Forces.Remove(force);
        }
    }
}