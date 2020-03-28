namespace POC3D.Model
{
    public interface IModelMaterial : IEntity
    {
        string Name { get; set; }

        double YoungsModulus { get; set; }
    }
}
