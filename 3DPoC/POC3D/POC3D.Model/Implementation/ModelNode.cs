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

        public bool IsXFixed { get; set; }

        public bool IsYFixed { get; set; }

        public bool IsZFixed { get; set; }
    }
}