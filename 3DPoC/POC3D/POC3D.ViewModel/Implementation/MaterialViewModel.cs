using POC3D.Model;
using POC3D.ViewModel.Base;

namespace POC3D.ViewModel.Implementation
{
    public class MaterialViewModel : SelectableViewModel
    {
        public MaterialViewModel(IModelMaterial modelMaterial)
        {
            ModelMaterial = modelMaterial;
        }

        public int Id => ModelMaterial.Id;

        public IModelMaterial ModelMaterial { get; }

        public string Name
        {
            get => ModelMaterial.Name;
            set
            {
                ModelMaterial.Name = value;
                OnPropertyChanged(nameof(Name));
            }
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