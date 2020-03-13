namespace POC3D.Model
{
    public abstract class Entity : IEntity
    {
        protected Entity(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public void SetId(int newId)
        {
            Id = newId;
        }
    }
}
