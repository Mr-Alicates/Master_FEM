namespace POC3D.Model
{
    public class ModelMaterial
    {
        public static readonly ModelMaterial None = new ModelMaterial("None", 1, 1);

        public ModelMaterial(string name, double youngsModulus, double poissonRatio)
        {
            Name = name;
            YoungsModulus = youngsModulus;
            PoissonRatio = poissonRatio;
        }

        public string Name { get; }

        public double YoungsModulus { get; }

        public double PoissonRatio { get; }
    }
}