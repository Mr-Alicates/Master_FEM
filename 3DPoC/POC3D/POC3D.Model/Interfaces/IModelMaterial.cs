namespace POC3D.Model
{
    public interface IModelMaterial
    {
        string Name { get; }

        double YoungsModulus { get; }

        double PoissonRatio { get; }
    }
}
