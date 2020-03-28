namespace POC3D.Model
{
    public class ModelBarElement : Entity, IModelElement
    {
        private IModelNode _originNode;
        private IModelNode _destinationNode;
        private IModelMaterial _material;

        public ModelBarElement(ModelProblem modelProblem, int id, IModelNode origin, IModelNode destination)
            : base(modelProblem, id)
        {
            OriginNode = origin;
            DestinationNode = destination;
        }

        public IModelNode OriginNode
        {
            get => _originNode;
            set
            {
                _originNode = value;
                ModelProblem.ValidateElements();
            }
        }

        public IModelNode DestinationNode
        {
            get => _destinationNode;
            set
            {
                _destinationNode = value;
                ModelProblem.ValidateElements();
            }
        }

        public ModelVector Direction => new ModelVector(DestinationNode.Coordinates, OriginNode.Coordinates);

        public IModelMaterial Material
        {
            get => _material;
            set
            {
                _material = value;
                ModelProblem.ValidateElements();
            }
        }

        public double CrossSectionArea { get; set; }

        public double Length => Direction.Modulus;

        public double K => Material.YoungsModulus * CrossSectionArea / Length;
    }
}