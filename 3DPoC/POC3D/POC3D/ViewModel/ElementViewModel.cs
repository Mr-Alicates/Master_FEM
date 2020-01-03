using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Helpers;
using POC3D.Model;
using POC3D.Model.Calculations;
using Matrix = POC3D.Model.Calculations.Matrix;

namespace POC3D.ViewModel
{
    public class ElementViewModel : Observable
    {
        private static readonly Brush BarBrush = Brushes.Blue;
        private static readonly Brush SelectedBarBrush = Brushes.Red;
        private NodeViewModel _destination;
        private bool _isSelected;
        private DiffuseMaterial _material;
        private MaterialViewModel _materialViewModel;
        private MeshGeometry3D _meshGeometry3D;
        private NodeViewModel _origin;

        public ElementViewModel(IModelElement modelElement, NodeViewModel origin, NodeViewModel destination)
        {
            Element = modelElement;
            _origin = origin;
            _destination = destination;
            _materialViewModel = new MaterialViewModel(Element.Material);

            Geometry = BuildGeometry();
            UpdateGeometry();

            Origin.PropertyChanged += NodesChanged;
            Destination.PropertyChanged += NodesChanged;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                UpdateGeometry();
            }
        }

        public IModelElement Element { get; }

        public NodeViewModel Origin
        {
            get => _origin;
            set
            {
                _origin = value;

                Element.OriginNode = _origin.Node;
                UpdateGeometry();
            }
        }

        public NodeViewModel Destination
        {
            get => _destination;
            set
            {
                _destination = value;

                Element.DestinationNode = _destination.Node;
                UpdateGeometry();
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

        public GeometryModel3D Geometry { get; }

        public Vector3D Direction => new Vector3D(Element.Direction.X, Element.Direction.Y, Element.Direction.Z);

        public double LocalCoordinateSystemRotationAngleA => Element.LocalCoordinateSystemRotationAngles.Alpha;

        public double LocalCoordinateSystemRotationAngleB => Element.LocalCoordinateSystemRotationAngles.Beta;

        public Matrix TransformationMatrix => Element.TransformationMatrix;

        public Matrix TransformationMatrixTransposed => Element.TransformationMatrix.Transpose();

        public Matrix LocalStiffnessMatrix => Element.LocalStiffnessMatrix;

        public Matrix GlobalStiffnessMatrix => Element.GlobalStiffnessMatrix;

        private void NodesChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Origin.Geometry)) UpdateGeometry();
        }

        private GeometryModel3D BuildGeometry()
        {
            _meshGeometry3D = new MeshGeometry3D
            {
                Positions = new Point3DCollection(),
                TriangleIndices = new Int32Collection()
            };

            _material = new DiffuseMaterial(BarBrush);

            return new GeometryModel3D
            {
                Material = _material,
                Geometry = _meshGeometry3D
            };
        }

        private void UpdateGeometry()
        {
            _material.Brush = IsSelected ? SelectedBarBrush : BarBrush;

            var vector = Destination.Coordinates - Origin.Coordinates;

            GraphicsHelper.BuildBarMesh(_meshGeometry3D, vector.Length, 0.5);

            var verticalVector = new Vector3D(0, 0, 1);
            var rotationAngle = Vector3D.AngleBetween(verticalVector, vector);
            var rotationVector = Vector3D.CrossProduct(verticalVector, vector);

            Geometry.Transform = new Transform3DGroup
            {
                Children = new Transform3DCollection
                {
                    new RotateTransform3D(new AxisAngleRotation3D(rotationVector, rotationAngle)),
                    new TranslateTransform3D(Origin.X, Origin.Y, Origin.Z)
                }
            };

            OnPropertyChanged(nameof(Geometry));
            OnPropertyChanged(nameof(Length));
            OnPropertyChanged(nameof(K));
            OnPropertyChanged(nameof(LocalCoordinateSystemRotationAngleA));
            OnPropertyChanged(nameof(LocalCoordinateSystemRotationAngleB));
            OnPropertyChanged(nameof(TransformationMatrix));
        }
    }
}