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
        private readonly List<IModelMaterial> _materials = new List<IModelMaterial>();

        public ModelProblem(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<IModelNode> Nodes => _nodes;

        public IEnumerable<IModelElement> Elements => _elements;
        
        public IEnumerable<IModelForce> Forces => _forces;

        public IEnumerable<IModelMaterial> Materials => _materials;

        public IModelNode AddNode()
        {
            var id = GetNextId(_nodes);
            var result = new ModelNode(this, id);

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

            if (origin == destination)
            {
                throw new InvalidOperationException($"Nodes {nameof(origin)} and {nameof(destination)} are the same node");
            }

            if (Elements.Any(other => other.OriginNode == origin && other.DestinationNode == destination ||
                                      other.OriginNode == destination && other.DestinationNode == origin ))
            {
                throw new InvalidOperationException($"An element with nodes {origin.Id} and {destination.Id} already exists in current problem");
            }

            var id = GetNextId(_elements);
            var element = new ModelBarElement(this, id, origin, destination)
            {
                Material = GetOrCreateDefaultMaterial()
            };

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
            var force = new ModelForce(this, id, node);

            _forces.Add(force);
            return force;
        }

        public IModelMaterial AddMaterial()
        {
            var materialId = GetNextId(_materials);
            var material = new ModelMaterial(this, materialId, $"Material{materialId}", 1);

            _materials.Add(material);
            EnsureEntitiesAreSorted(_materials);
            return material;
        }

        public void DeleteNode(IModelNode node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (_elements.Any(element => element.OriginNode == node ||
                                        element.DestinationNode == node))
            {
                throw new InvalidOperationException($"Node {node.Id} cannot be deleted. It is being used by one or more elements");
            }

            if (_forces.Any(force => force.Node == node))
            {
                throw new InvalidOperationException($"Node {node.Id} cannot be deleted. It is being used by one or more forces");
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

        public void DeleteMaterial(IModelMaterial material)
        {
            if (material is null)
            {
                throw new ArgumentNullException(nameof(material));
            }

            if(_elements.Any(element => element.Material == material))
            {
                throw new InvalidOperationException($"Material {material.Name} cannot be deleted. It is being used by one or more elements");
            }

            _materials.Remove(material);
            EnsureEntitiesAreSorted(_materials);
        }
        
        public void ValidateForces()
        {
            //Ensure there is only one force applied on each node
            var invalidForces = _forces.GroupBy(force => force.Node)
                .Any(group => group.Count() > 1);

            if (invalidForces)
            {
                throw new InvalidOperationException("There is more than one force applied on the same node. Problem is invalid");
            }

            //Ensure there are no forces with stray nodes
            var invalidForceNodes = _forces.Select(force => force.Node)
                .Any(node => !_nodes.Contains(node));

            if (invalidForceNodes)
            {
                throw new InvalidOperationException("There is one or more forces whose application nodes are invalid. Problem is invalid");
            }
        }

        public void ValidateElements()
        {
            //Ensure there are no elements whose origin and destination nodes are the same
            var zeroLengthElements = _elements.Any(element => element.OriginNode == element.DestinationNode);

            if (zeroLengthElements)
            {
                throw new InvalidOperationException($"There is one or more elements whose {nameof(IModelElement.OriginNode)} and {nameof(IModelElement.OriginNode)} are the same. Problem is invalid");
            }

            //Ensure there are no elements with the same nodes
            foreach (var element in _elements)
            {
                foreach (var otherElement in _elements)
                {
                    if (otherElement.Id == element.Id)
                    {
                        continue;
                    }

                    if (otherElement.OriginNode == element.OriginNode &&
                        otherElement.DestinationNode == element.DestinationNode ||
                        otherElement.OriginNode == element.DestinationNode &&
                        otherElement.DestinationNode == element.OriginNode)
                    {
                        throw new InvalidOperationException("There is one or more duplicate elements (with the same set of nodes). Problem is invalid");
                    }
                }
            }

            //Ensure there are no elements with stray nodes
            var invalidElementNodes = _elements.Select(x => x.OriginNode)
                .Union(_elements.Select(x => x.DestinationNode))
                .Any(node => !_nodes.Contains(node));

            if (invalidElementNodes)
            {
                throw new InvalidOperationException("There is more than one elements whose nodes are invalid. Problem is invalid");
            }

            //Ensure there are no stray materials
            var invalidMaterials = _elements.Select(element => element.Material)
                .Any(material => !_materials.Contains(material));

            if (invalidMaterials)
            {
                throw new InvalidOperationException("There is one or more elements whose materials are invalid. Problem is invalid");
            }

        }

        private IModelMaterial GetOrCreateDefaultMaterial()
        {
            if (_materials.Any())
            {
                return _materials.First();
            }

            return AddMaterial();
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