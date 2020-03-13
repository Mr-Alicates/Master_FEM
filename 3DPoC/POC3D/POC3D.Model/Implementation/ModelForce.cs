namespace POC3D.Model
{
    public class ModelForce : Entity, IModelForce
    {
        public ModelForce(int id, IModelNode applicationNode)
            : base(id)
        {
            Node = applicationNode;
            ApplicationVector = new ModelVector();
        }

        public IModelNode Node { get; set; }

        public ModelVector ApplicationVector { get; }

        public double Magnitude => ApplicationVector.Modulus;
    }
}