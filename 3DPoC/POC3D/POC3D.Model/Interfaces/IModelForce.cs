namespace POC3D.Model
{
    public interface IModelForce : IEntity
    {
        IModelNode Node { get; set; }

        ModelVector ApplicationVector { get; }

        double Magnitude { get; }
    }
}
