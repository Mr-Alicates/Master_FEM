namespace POC3D.Model
{
    public class ModelBarElement : IModelElement
    {
        //I will cleanup this later;
        private static int IdCounter = 1;

        public ModelBarElement(IModelNode origin, IModelNode destination)
        {
            OriginNode = origin;
            DestinationNode = destination;
            Id = IdCounter++;
        }

        public int Id { get; }

        public string Description => $"({OriginNode.Id}) ---> ({DestinationNode.Id})";

        public IModelNode OriginNode { get; set; }

        public IModelNode DestinationNode { get; set; }

        public ModelVector Direction => new ModelVector(DestinationNode.Coordinates, OriginNode.Coordinates);

        public IModelMaterial Material { get; set; } = ModelMaterial.None;

        public double CrossSectionArea { get; set; }

        public double Length => Direction.Modulus;

        public double K => Material.YoungsModulus * CrossSectionArea / Length;
    }
}