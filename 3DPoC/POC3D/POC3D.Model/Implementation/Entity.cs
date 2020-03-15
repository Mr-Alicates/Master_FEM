namespace POC3D.Model
{
    public abstract class Entity : IEntity
    {
        protected Entity(ModelProblem modelProblem, int id)
        {
            ModelProblem = modelProblem;
            Id = id;
        }

        public int Id { get; private set; }

        public void SetId(int newId)
        {
            Id = newId;
        }

        public ModelProblem ModelProblem { get; }
    }
}
