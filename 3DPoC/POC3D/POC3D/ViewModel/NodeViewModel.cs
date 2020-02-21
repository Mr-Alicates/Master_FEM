using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Helpers;
using POC3D.Model;

namespace POC3D.ViewModel
{
    public class NodeViewModel : Observable
    {
        private static readonly Brush FreeNodeBrush = Brushes.LightGreen;
        private static readonly Brush FixedNodeBrush = Brushes.DarkGreen;
        private static readonly Brush SelectedNodeBrush = Brushes.Red;

        private bool _isSelected;
        private DiffuseMaterial _material;
        private MeshGeometry3D _meshGeometry3D;
        private TranslateTransform3D _translateTransform3D;

        public NodeViewModel(ModelNode modelNode)
        {
            Node = modelNode;
            BuildGeometry();
        }

        public int Id => Node.Id;

        public ModelNode Node { get; }

        public Point3D Coordinates => new Point3D(
            Node.Coordinates.X,
            Node.Coordinates.Y,
            Node.Coordinates.Z);

        public double X
        {
            get => Node.Coordinates.X;
            set
            {
                Node.Coordinates.X = value;
                _translateTransform3D.OffsetX = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public double Y
        {
            get => Node.Coordinates.Y;
            set
            {
                Node.Coordinates.Y = value;
                _translateTransform3D.OffsetY = value;
                OnPropertyChanged(nameof(Y));
            }
        }

        public double Z
        {
            get => Node.Coordinates.Z;
            set
            {
                Node.Coordinates.Z = value;
                _translateTransform3D.OffsetZ = value;
                OnPropertyChanged(nameof(Z));
            }
        }

        public bool IsFixed
        {
            get => Node.IsFixed;
            set
            {
                Node.IsFixed = value;

                if (IsFixed)
                {
                    GraphicsHelper.BuildPyramidMesh(_meshGeometry3D, 2);
                }
                else
                {
                    GraphicsHelper.BuildCubeMesh(_meshGeometry3D, 1);
                }

                OnPropertyChanged(nameof(IsFixed));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                _material.Brush = IsSelected ? SelectedNodeBrush : IsFixed ? FixedNodeBrush : FreeNodeBrush;
            }
        }

        public GeometryModel3D Geometry { get; private set; }

        public string Name => $"{Id} ({Coordinates.ToString()})";

        public NodeViewModel SetAsFixed()
        {
            IsFixed = true;
            return this;
        }

        public NodeViewModel SetAsFree()
        {
            IsFixed = false;
            return this;
        }

        private void BuildGeometry()
        {
            _meshGeometry3D = new MeshGeometry3D
            {
                Positions = new Point3DCollection(),
                TriangleIndices = new Int32Collection()
            };

            _material = new DiffuseMaterial(FreeNodeBrush);

            _translateTransform3D = new TranslateTransform3D(X, Y, Z);

            Geometry = new GeometryModel3D
            {
                Material = _material,
                Geometry = _meshGeometry3D,
                Transform = _translateTransform3D
            };
        }
    }
}