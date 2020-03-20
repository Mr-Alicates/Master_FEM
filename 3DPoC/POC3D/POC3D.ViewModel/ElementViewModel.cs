using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Calculations;
using POC3D.ViewModel.Geometry;

namespace POC3D.ViewModel
{
    public class ElementViewModel : SelectableViewModel
    {
        private double? _cx;
        private double? _cy;
        private double? _cz;
        private NumericMatrix _transformationMatrix;
        private NumericMatrix _transformationMatrixTransposed;
        private NumericMatrix _localStiffnessMatrix;
        private NumericMatrix _globalStiffnessMatrix;
        
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
        }

        public IModelElement Element { get; }

        public NodeViewModel Origin
        {
            get => _origin;
            set
            {
                _origin = value;
                Element.OriginNode = _origin.Node;
                InvalidateCaches();
            }
        }

        public NodeViewModel Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                Element.DestinationNode = _destination.Node;
                InvalidateCaches();
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
                InvalidateCaches();
            }
        }

        public double CrossSectionArea
        {
            get => Element.CrossSectionArea;
            set
            {
                Element.CrossSectionArea = value;
                InvalidateCaches();
            }
        }

        public double Length => Element.Length;

        public string K => Element.K.ToString("E2");

        public NumericMatrix TransformationMatrix =>
            _transformationMatrix ??= MatrixHelper.BuildTransformationMatrix(this);

        public NumericMatrix TransformationMatrixTransposed =>
            _transformationMatrixTransposed ??= TransformationMatrix.Transpose();

        public NumericMatrix LocalStiffnessMatrix =>
            _localStiffnessMatrix ??= MatrixHelper.BuildElementLocalStiffnessMatrix(this);

        public NumericMatrix GlobalStiffnessMatrix =>
            _globalStiffnessMatrix ??= MatrixHelper.BuildElementGlobalStiffnessMatrix(this);

        public double Cx => _cx ??= (Destination.Coordinates.X - Origin.Coordinates.X) / Length;

        public double Cy => _cy ??= (Destination.Coordinates.Y - Origin.Coordinates.Y) / Length;

        public double Cz => _cz ??= (Destination.Coordinates.Z - Origin.Coordinates.Z) / Length;

        public GeometryModel3D Geometry => _elementGeometryViewModel.Geometry;

        private void InvalidateCaches()
        {
            _cx = null;
            _cy = null;
            _cz = null;

            _transformationMatrix = null;
            _transformationMatrixTransposed = null;
            _localStiffnessMatrix = null;
            _globalStiffnessMatrix = null;

            OnPropertyChanged(nameof(Material));
            OnPropertyChanged(nameof(Origin));
            OnPropertyChanged(nameof(Destination));
            OnPropertyChanged(nameof(Length));
            OnPropertyChanged(nameof(CrossSectionArea));
            OnPropertyChanged(nameof(K));
            OnPropertyChanged(nameof(Cx));
            OnPropertyChanged(nameof(Cy));
            OnPropertyChanged(nameof(Cz));
            OnPropertyChanged(nameof(TransformationMatrix));
            OnPropertyChanged(nameof(TransformationMatrixTransposed));
            OnPropertyChanged(nameof(LocalStiffnessMatrix));
            OnPropertyChanged(nameof(GlobalStiffnessMatrix));
        }
    }
}