using System;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Calculations;
using POC3D.ViewModel.Geometry;

namespace POC3D.ViewModel
{
    public class ElementViewModel : SelectableViewModel
    {
        private MaterialViewModel _materialViewModel;
        private NodeViewModel _origin;
        private NodeViewModel _destination;

        private readonly ElementGeometryViewModel _elementGeometryViewModel;

        public ElementViewModel(IModelElement modelElement, NodeViewModel origin, NodeViewModel destination, MaterialViewModel materialViewModel)
        {
            Element = modelElement;
            Origin = origin;
            Destination = destination;
            Material = materialViewModel;

            _elementGeometryViewModel = new ElementGeometryViewModel(this);
            ElementCalculationViewModel = new ElementCalculationViewModel(this);
        }

        public IModelElement Element { get; }

        public NodeViewModel Origin
        {
            get => _origin;
            set
            {
                if (_origin != null) _origin.PropertyChanged -= (_, __) => OnPropertyChanged(nameof(Origin));

                value.PropertyChanged += (_, __) => OnPropertyChanged(nameof(Origin)); ;
                _origin = value;
                Element.OriginNode = _origin.Node;
                OnPropertyChanged(nameof(Origin));
                OnPropertyChanged(nameof(Length));
                OnPropertyChanged(nameof(K));
            }
        }

        public NodeViewModel Destination
        {
            get => _destination;
            set
            {
                if (_destination != null) _destination.PropertyChanged -= (_, __) => OnPropertyChanged(nameof(Destination));

                value.PropertyChanged += (_, __) => OnPropertyChanged(nameof(Destination));
                _destination = value;
                Element.DestinationNode = _destination.Node;
                OnPropertyChanged(nameof(Destination));
                OnPropertyChanged(nameof(Length));
                OnPropertyChanged(nameof(K));
            }
        }

        public int Id => Element.Id;

        public string Description => Element.Description;

        public MaterialViewModel Material
        {
            get => _materialViewModel;
            set
            {
                _materialViewModel = value;
                Element.Material = _materialViewModel.ModelMaterial;
                OnPropertyChanged(nameof(Material));
                OnPropertyChanged(nameof(K));
            }
        }

        public double CrossSectionArea
        {
            get => Element.CrossSectionArea;
            set
            {
                Element.CrossSectionArea = value;
                OnPropertyChanged(nameof(CrossSectionArea));
                OnPropertyChanged(nameof(K));
            }
        }

        public double Length => Element.Length;

        public string K => Element.K.ToString("E2");

        public GeometryModel3D Geometry => _elementGeometryViewModel.Geometry;

        public ElementCalculationViewModel ElementCalculationViewModel { get; }
    }
}