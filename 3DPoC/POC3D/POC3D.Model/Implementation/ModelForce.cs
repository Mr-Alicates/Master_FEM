namespace POC3D.Model
{
    public class ModelForce : IModelForce
    {
        public ModelForce(IModelNode applicationNode)
        {
            Node = applicationNode;
            ApplicationVector = new ModelVector();
        }

        public IModelNode Node { get; set; }

        public ModelVector ApplicationVector { get; }

        public double Magnitude => ApplicationVector.Modulus;
    }
}