﻿namespace POC3D.Model
{
    public class ModelBarElement : IModelElement
    {
        //I will cleanup this later;
        private static int IdCounter = 1;

        public ModelBarElement(ModelNode origin, ModelNode destination)
        {
            OriginNode = origin;
            DestinationNode = destination;
            Id = IdCounter++;
        }

        public int Id { get; }

        public string Description => $"({OriginNode.Id}) ---> ({DestinationNode.Id})";

        public ModelNode OriginNode { get; set; }

        public ModelNode DestinationNode { get; set; }

        public ModelMaterial Material { get; set; } = ModelMaterial.None;

        public double CrossSectionArea { get; set; }

        public double Length => new ModelVector(DestinationNode.Coordinates, OriginNode.Coordinates).Modulus;

        public double K => Material.YoungsModulus * CrossSectionArea / Length;
    }
}