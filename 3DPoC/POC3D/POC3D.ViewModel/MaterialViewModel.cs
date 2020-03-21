using POC3D.Model;
using POC3D.ViewModel.Base;

namespace POC3D.ViewModel
{
    public class MaterialViewModel : SelectableViewModel
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
            set
            {
                ModelMaterial.YoungsModulus = value;
                OnPropertyChanged(nameof(YoungsModulus));
            }
        }
    }
}