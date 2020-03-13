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

        public IModelElement AddElement(IModelNode node1, IModelNode node2)
        {
            if (!Nodes.Contains(node1) || !Nodes.Contains(node2))
            {
                throw new InvalidOperationException();
            }

            var id = GetNextId(_elements);
            var element = new ModelBarElement(id, node1, node2);

            _elements.Add(element);
            return element;
        }

        public IModelForce AddForce(IModelNode node)
        {
            if (!_nodes.Contains(node))
            {
                throw new InvalidOperationException();
            }

            var id = GetNextId(_nodes);
            var force = new ModelForce(id, node);

            _forces.Add(force);
            return force;
        }

        public void DeleteNode(IModelNode selectedNodeNode)
        {
            _nodes.Remove(selectedNodeNode);
            EnsureEntitiesAreSorted(_nodes);
        }

        public void DeleteElement(IModelElement element)
        {
            _elements.Remove(element);
            EnsureEntitiesAreSorted(_elements);
        }

        public void DeleteForce(IModelForce force)
        {
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