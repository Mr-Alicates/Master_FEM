using POC3D.Helpers;
using POC3D.Model.Calculations;

namespace POC3D.Model
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

        public ModelVector Direction => new ModelVector(DestinationNode.Coordinates, OriginNode.Coordinates);

        public ModelMaterial Material { get; set; } = ModelMaterial.None;

        public double CrossSectionArea { get; set; }

        public double Length => Direction.Modulus;

        public double K => Material.YoungsModulus * CrossSectionArea / Length;

        public NumericMatrix TransformationMatrix => MatrixHelper.BuildTransformationMatrix(this);

        public NumericMatrix LocalStiffnessMatrix => MatrixHelper.BuildElementLocalStiffnessMatrix(this);

        public NumericMatrix GlobalStiffnessMatrix => MatrixHelper.BuildElementGlobalStiffnessMatrix(this);

        public double Cx => (DestinationNode.Coordinates.X - OriginNode.Coordinates.X) / Length;

        public double Cy => (DestinationNode.Coordinates.Y - OriginNode.Coordinates.Y) / Length;

        public double Cz => (DestinationNode.Coordinates.Z - OriginNode.Coordinates.Z) / Length;
    }
}