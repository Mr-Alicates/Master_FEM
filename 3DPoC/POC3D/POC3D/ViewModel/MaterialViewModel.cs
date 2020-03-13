using POC3D.Model;

namespace POC3D.ViewModel
{
    public class MaterialViewModel
    {
        public MaterialViewModel(IModelMaterial modelMaterial)
        {
            ModelMaterial = modelMaterial;
        }

        public IModelMaterial ModelMaterial { get; }

        public string Name => ModelMaterial.Name;

        public double YoungsModulus => ModelMaterial.YoungsModulus;

        public double PoissonRatio => ModelMaterial.PoissonRatio;

        public override bool Equals(object obj)
        {
            return obj is MaterialViewModel other && Equals(other);
        }

        protected bool Equals(MaterialViewModel other)
        {
            return Equals(ModelMaterial, other.ModelMaterial);
        }

        public override int GetHashCode()
        {
            return ModelMaterial != null ? ModelMaterial.GetHashCode() : 0;
        }
    }
}