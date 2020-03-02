using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Helpers;
using POC3D.Model;
using POC3D.Model.Calculations;

namespace POC3D.ViewModel
{
    public class ElementViewModel : GeometryViewModel
    {
        private static readonly Vector3D VerticalVector = new Vector3D(0, 0, 1);
        private static readonly Brush BarBrush = Brushes.Blue;
        private static readonly Brush SelectedBarBrush = Brushes.Red;
        private double? _cx;
        private double? _cy;
        private double? _cz;
        private NodeViewModel _destination;
        private NumericMatrix _globalStiffnessMatrix;

        private bool _isSelected;
        private NumericMatrix _localStiffnessMatrix;
        private MaterialViewModel _materialViewModel;
        private NodeViewModel _origin;

        private NumericMatrix _transformationMatrix;
        private NumericMatrix _transformationMatrixTransposed;

        public ElementViewModel(IModelElement modelElement, NodeViewModel origin, NodeViewModel destination)
        {
            Element = modelElement;
            Origin = origin;
            Destination = destination;
            _materialViewModel = new MaterialViewModel(Element.Material);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                MaterialBrush = IsSelected ? SelectedBarBrush : BarBrush;
            }
        }

        public IModelElement Element { get; }

        public NodeViewModel Origin
        {
            get => _origin;
            set
            {
                if (_origin != null) _origin.PropertyChanged -= NodesChanged;

                value.PropertyChanged += NodesChanged;
                _origin = value;

                Element.OriginNode = _origin.Node;
                UpdateGeometryMesh();
            }
        }

        public NodeViewModel Destination
        {
            get => _destination;
            set
            {
                if (_destination != null) _destination.PropertyChanged -= NodesChanged;

                value.PropertyChanged += NodesChanged;
                _destination = value;

                Element.DestinationNode = _destination.Node;
                UpdateGeometryMesh();
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
                OnPropertyChanged(nameof(LocalStiffnessMatrix));
                OnPropertyChanged(nameof(GlobalStiffnessMatrix));
            }
        }

        public double CrossSectionArea
        {
            get => Element.CrossSectionArea;
            set
            {
                Element.CrossSectionArea = value;
                UpdateGeometryMesh();
                OnPropertyChanged(nameof(CrossSectionArea));
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

        private void NodesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_origin.Coordinates)) UpdateGeometryMesh();
        }

        protected override void UpdateGeometryMesh()
        {
            if (Destination == null || Origin == null) return;

            var vector = Destination.Coordinates - Origin.Coordinates;

            GraphicsHelper.BuildBarMesh(MeshGeometry3D, vector.Length, 0.5);

            RotationAngle = Vector3D.AngleBetween(VerticalVector, vector);
            RotationAxis = Vector3D.CrossProduct(VerticalVector, vector);

            OffsetX = Origin.X;
            OffsetY = Origin.Y;
            OffsetZ = Origin.Z;

            _cx = null;
            _cy = null;
            _cz = null;

            _transformationMatrix = null;
            _transformationMatrixTransposed = null;
            _localStiffnessMatrix = null;
            _globalStiffnessMatrix = null;

            OnPropertyChanged(nameof(Origin));
            OnPropertyChanged(nameof(Destination));
            OnPropertyChanged(nameof(Length));
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