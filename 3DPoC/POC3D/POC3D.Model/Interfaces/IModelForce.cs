namespace POC3D.Model
{
    public interface IModelForce
    {
        IModelNode Node { get; set; }

        ModelVector ApplicationVector { get; }

        double Magnitude { get; }
    }
}
