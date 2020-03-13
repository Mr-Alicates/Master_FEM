namespace POC3D.Model
{
    public class ModelNode : Entity, IModelNode
    {
        public ModelNode(int id)
            : base(id)
        {
            Coordinates = new ModelPoint();
        }

        public ModelPoint Coordinates { get; }

        public bool IsFixed { get; set; }
    }
}