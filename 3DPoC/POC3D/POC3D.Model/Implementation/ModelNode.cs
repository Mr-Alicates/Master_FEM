namespace POC3D.Model
{
    public class ModelNode : Entity, IModelNode
    {
        public ModelNode(ModelProblem modelProblem, int id)
            : base(modelProblem, id)
        {
            Coordinates = new ModelPoint();
        }

        public ModelPoint Coordinates { get; }

        public bool IsFixed { get; set; }
    }
}