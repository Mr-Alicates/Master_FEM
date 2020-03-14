namespace POC3D.Model
{
    public class ModelBarElement : Entity, IModelElement
    {
        public ModelBarElement(int id, IModelNode origin, IModelNode destination)
            : base(id)
        {
            OriginNode = origin;
            DestinationNode = destination;
        }

        public string Description => $"({OriginNode.Id}) ---> ({DestinationNode.Id})";

        public IModelNode OriginNode { get; set; }

        public IModelNode DestinationNode { get; set; }

        public ModelVector Direction => new ModelVector(DestinationNode.Coordinates, OriginNode.Coordinates);

        public IModelMaterial Material { get; set; }

        public double CrossSectionArea { get; set; }

        public double Length => Direction.Modulus;

        public double K => Material.YoungsModulus * CrossSectionArea / Length;
    }
}