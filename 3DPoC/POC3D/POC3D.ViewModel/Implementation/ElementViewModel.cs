using System;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Calculations;
using POC3D.ViewModel.Geometry;

namespace POC3D.ViewModel.Implementation
{
    public class ElementViewModel : SelectableViewModel
    {
        private MaterialViewModel _materialViewModel;
        private NodeViewModel _origin;
        private NodeViewModel _destination;

        private readonly ElementGeometryViewModel _elementGeometryViewModel;
        private readonly ResultElementGeometryViewModel _resultElementGeometryViewModel;

        public ElementViewModel(IModelElement modelElement, NodeViewModel origin, NodeViewModel destination, MaterialViewModel materialViewModel)
        {
            Element = modelElement;

            _origin = origin;
            _origin.PropertyChanged += NodeChanged;
            _destination = destination;
            _destination.PropertyChanged += NodeChanged;

            _materialViewModel = materialViewModel;
            _materialViewModel.PropertyChanged += MaterialChanged;

            _elementGeometryViewModel = new ElementGeometryViewModel(this);
            _resultElementGeometryViewModel = new ResultElementGeometryViewModel(this);
            ElementCalculationViewModel = new ElementCalculationViewModel(this);
        }

        public IModelElement Element { get; }

        public NodeViewModel Origin
        {
            get => _origin;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Origin));
                }

                _origin.PropertyChanged -= NodeChanged;
                _origin = value;
                _origin.PropertyChanged += NodeChanged;

                Element.OriginNode = _origin.Node;
                OnPropertyChanged(nameof(Origin));
                OnPropertyChanged(nameof(Length));
                OnPropertyChanged(nameof(K));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Geometry));
                OnPropertyChanged(nameof(ResultGeometry));
            }
        }

        public NodeViewModel Destination
        {
            get => _destination;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Origin));
                }

                _destination.PropertyChanged -= NodeChanged;
                _destination = value;
                _destination.PropertyChanged += NodeChanged;

                Element.DestinationNode = _destination.Node;
                OnPropertyChanged(nameof(Destination));
                OnPropertyChanged(nameof(Length));
                OnPropertyChanged(nameof(K));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Geometry));
                OnPropertyChanged(nameof(ResultGeometry));
            }
        }

        public int Id => Element.Id;

        public string Description => $"({Origin.Id}) ---> ({Destination.Id})";

        public MaterialViewModel Material
        {
            get => _materialViewModel;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Origin));
                }

                _materialViewModel.PropertyChanged -= MaterialChanged;
                _materialViewModel = value;
                _materialViewModel.PropertyChanged += MaterialChanged;

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

        public GeometryModel3D ResultGeometry => _resultElementGeometryViewModel.Geometry;

        public ElementCalculationViewModel ElementCalculationViewModel { get; }

        private void MaterialChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MaterialViewModel.YoungsModulus))
            {
                OnPropertyChanged(nameof(Material));
                OnPropertyChanged(nameof(K));
            }
        }

        private void NodeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NodeViewModel.Coordinates) ||
                e.PropertyName == nameof(NodeViewModel.IsFixed))
            {
                if (sender == Origin)
                {
                    OnPropertyChanged(nameof(Origin));
                }
                else
                {
                    OnPropertyChanged(nameof(Destination));
                }
            }
        }
    }
}