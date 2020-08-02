using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POC3D.Model.Serialization
{
    public class ProblemSerializer : IProblemSerializer
    {
        private readonly IFileSystem _fileSystem;

        public ProblemSerializer(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void SerializeProblem(IModelProblem modelProblem, string filePath)
        {
            if (modelProblem is null)
            {
                throw new ArgumentNullException(nameof(modelProblem));
            }

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(nameof(filePath));
            }

            var directoryPath = Path.GetDirectoryName(filePath);

            if (!_fileSystem.DirectoryExists(directoryPath))
            {
                _fileSystem.CreateDirectory(directoryPath);
            }

            if (_fileSystem.FileExists(filePath))
            {
                _fileSystem.FileDelete(filePath);
            }

            var memento = BuildMemento(modelProblem);

            var json = JsonConvert.SerializeObject(memento);

            _fileSystem.FileWrite(filePath, json);
        }

        public IModelProblem DeserializeProblem(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException(nameof(filePath));
            }

            if (!_fileSystem.FileExists(filePath))
            {
                throw new InvalidOperationException($"Invalid path {filePath}");
            }

            try
            {
                var json = _fileSystem.FileRead(filePath);

                var memento = JsonConvert.DeserializeObject<ProblemMemento>(json);

                return BuildProblem(memento);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error reading file {filePath}", ex);
            }
        }

        private static ProblemMemento BuildMemento(IModelProblem modelProblem)
        {
            var result = new ProblemMemento()
            {
                Name = modelProblem.Name
            };

            foreach (var modelNode in modelProblem.Nodes)
            {
                var nodeMemento = new NodeMemento()
                {
                    Id = modelNode.Id,
                    IsXFixed = modelNode.IsXFixed,
                    IsYFixed = modelNode.IsYFixed,
                    IsZFixed = modelNode.IsZFixed,
                    X = modelNode.Coordinates.X,
                    Y = modelNode.Coordinates.Y,
                    Z = modelNode.Coordinates.Z
                };

                result.Nodes.Add(nodeMemento);
            }

            foreach (var modelForce in modelProblem.Forces)
            {
                var forceMemento = new ForceMemento()
                {
                    Id = modelForce.Id,
                    ApplicationVectorX = modelForce.ApplicationVector.X,
                    ApplicationVectorY = modelForce.ApplicationVector.Y,
                    ApplicationVectorZ = modelForce.ApplicationVector.Z,
                    NodeId = modelForce.Node.Id
                };

                result.Forces.Add(forceMemento);
            }

            foreach (var modelMaterial in modelProblem.Materials)
            {
                var materialMemento = new MaterialMemento
                {
                    Id = modelMaterial.Id,
                    Name = modelMaterial.Name,
                    YoungsModulus = modelMaterial.YoungsModulus
                };

                result.Materials.Add(materialMemento);
            }

            foreach (var modelBarElement in modelProblem.Elements)
            {
                var barElementMemento = new BarElementMemento
                {
                    Id = modelBarElement.Id,
                    OriginNodeId = modelBarElement.OriginNode.Id,
                    DestinationNodeId = modelBarElement.DestinationNode.Id,
                    MaterialId = modelBarElement.Material.Id,
                    CrossSectionArea = modelBarElement.CrossSectionArea
                };

                result.Elements.Add(barElementMemento);
            }

            return result;
        }

        private static IModelProblem BuildProblem(ProblemMemento problemMemento)
        {
            var modelProblem = new ModelProblem(problemMemento.Name);

            Dictionary<int, IModelNode> nodesDictionary = new Dictionary<int, IModelNode>();
            Dictionary<int, IModelMaterial> materialsDictionary = new Dictionary<int, IModelMaterial>();

            foreach (var nodeMemento in problemMemento.Nodes.OrderBy(x => x.Id))
            {
                var modelNode = modelProblem.AddNode();

                modelNode.IsXFixed = nodeMemento.IsXFixed;
                modelNode.IsYFixed = nodeMemento.IsYFixed;
                modelNode.IsZFixed = nodeMemento.IsZFixed;
                modelNode.Coordinates.X = nodeMemento.X;
                modelNode.Coordinates.Y = nodeMemento.Y;
                modelNode.Coordinates.Z = nodeMemento.Z;

                nodesDictionary.Add(nodeMemento.Id, modelNode);
            }

            foreach (var forceMemento in problemMemento.Forces.OrderBy(x => x.Id))
            {
                var applicationNode = nodesDictionary[forceMemento.NodeId];

                var modelForce = modelProblem.AddForce(applicationNode);
                modelForce.ApplicationVector.X = forceMemento.ApplicationVectorX;
                modelForce.ApplicationVector.Y = forceMemento.ApplicationVectorY;
                modelForce.ApplicationVector.Z = forceMemento.ApplicationVectorZ;
            }

            foreach (var mementoMaterial in problemMemento.Materials.OrderBy(x => x.Id))
            {
                var modelMaterial = modelProblem.AddMaterial();

                modelMaterial.Name = mementoMaterial.Name;
                modelMaterial.YoungsModulus = mementoMaterial.YoungsModulus;

                materialsDictionary.Add(mementoMaterial.Id, modelMaterial);
            }

            foreach (var elementMemento in problemMemento.Elements.OrderBy(x => x.Id))
            {
                var modelMaterial = materialsDictionary[elementMemento.MaterialId];
                var originModelNode = nodesDictionary[elementMemento.OriginNodeId];
                var destinationModelNode = nodesDictionary[elementMemento.DestinationNodeId];

                var modelElement = modelProblem.AddElement(originModelNode, destinationModelNode);
                modelElement.Material = modelMaterial;
                modelElement.CrossSectionArea = elementMemento.CrossSectionArea;
            }

            return modelProblem;
        }
    }
}
