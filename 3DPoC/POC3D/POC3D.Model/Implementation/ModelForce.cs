namespace POC3D.Model
{
    public class ModelForce : Entity, IModelForce
    {
        private IModelNode _node;

        public ModelForce(ModelProblem modelProblem, int id, IModelNode applicationNode)
            : base(modelProblem, id)
        {
            _node = applicationNode;
            ApplicationVector = new ModelVector();
        }

        public IModelNode Node 
        { 
            get => _node;
            set 
            {
                _node = value;
                ModelProblem.ValidateForces();
            } 
        }

        public ModelVector ApplicationVector { get; }

        public double Magnitude => ApplicationVector.Modulus;
    }
}