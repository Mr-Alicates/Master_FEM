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

        public string Name
        { 
            get => ModelMaterial.Name;
            set => ModelMaterial.Name = value;
        }

        public double YoungsModulus
        {
            get => ModelMaterial.YoungsModulus;
            set => ModelMaterial.YoungsModulus = value;
        }

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