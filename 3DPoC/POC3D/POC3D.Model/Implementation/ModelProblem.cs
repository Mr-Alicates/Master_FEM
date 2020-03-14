using System;
using System.Collections.Generic;
using System.Linq;

namespace POC3D.Model
{
    public class ModelProblem : IModelProblem
    {
        private readonly List<IModelNode> _nodes = new List<IModelNode>();
        private readonly List<IModelElement> _elements = new List<IModelElement>();
        private readonly List<IModelForce> _forces = new List<IModelForce>();

        public ModelProblem(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<IModelNode> Nodes => _nodes;

        public IEnumerable<IModelElement> Elements => _elements;
        
        public IEnumerable<IModelForce> Forces => _forces;

        public IModelNode AddNode()
        {
            var id = GetNextId(_nodes);
            var result = new ModelNode(id);

            _nodes.Add(result);
            return result;
        }

        public IModelElement AddElement(IModelNode origin, IModelNode destination)
        {
            if (origin is null)
            {
                throw new ArgumentNullException(nameof(origin));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (!Nodes.Contains(origin))
            {
                throw new InvalidOperationException($"Node {nameof(origin)} does not exist in current problem");
            }

            if (!Nodes.Contains(destination))
            {
                throw new InvalidOperationException($"Node {nameof(destination)} does not exist in current problem");
            }

            if (Elements.Any(other => other.OriginNode == origin && other.DestinationNode == destination ||
                                      other.OriginNode == destination && other.DestinationNode == origin ))
            {
                throw new InvalidOperationException($"An element with nodes {origin.Id} and {destination.Id} already exists in current problem");
            }

            var id = GetNextId(_elements);
            var element = new ModelBarElement(id, origin, destination);

            _elements.Add(element);
            return element;
        }

        public IModelForce AddForce(IModelNode node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (!Nodes.Contains(node))
            {
                throw new InvalidOperationException();
            }

            if (Forces.Any(other => other.Node == node))
            {
                throw new InvalidOperationException($"A force with node {node.Id} already exists in current problem");
            }

            var id = GetNextId(_forces);
            var force = new ModelForce(id, node);

            _forces.Add(force);
            return force;
        }

        public void DeleteNode(IModelNode node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            _nodes.Remove(node);
            EnsureEntitiesAreSorted(_nodes);
        }

        public void DeleteElement(IModelElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            _elements.Remove(element);
            EnsureEntitiesAreSorted(_elements);
        }

        public void DeleteForce(IModelForce force)
        {
            if (force is null)
            {
                throw new ArgumentNullException(nameof(force));
            }

            _forces.Remove(force);
            EnsureEntitiesAreSorted(_forces);
        }

        private static int GetNextId(IEnumerable<IEntity> entities)
        {
            return entities.Any() ? entities.Max(x => x.Id) + 1 : 1;
        }

        private static void EnsureEntitiesAreSorted(IEnumerable<IEntity> entities)
        {
            int id = 1;
            foreach(var entity in entities)
            {
                entity.SetId(id);
                id++;
            }
        }
    }
}