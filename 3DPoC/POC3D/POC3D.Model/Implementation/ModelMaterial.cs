namespace POC3D.Model
{
    public class ModelMaterial : Entity, IModelMaterial
    {
        public ModelMaterial(int id, string name, double youngsModulus)
            : base(id)
        {
            Name = name;
            YoungsModulus = youngsModulus;
        }

        public string Name { get; set; }

        public double YoungsModulus { get; set; }
    }
}