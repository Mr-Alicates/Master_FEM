using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Helpers;
using POC3D.Model;

namespace POC3D.ViewModel
{
    public class NodeViewModel : GeometryViewModel
    {
        private static readonly Brush FreeNodeBrush = Brushes.LightGreen;
        private static readonly Brush FixedNodeBrush = Brushes.DarkGreen;
        private static readonly Brush SelectedNodeBrush = Brushes.Red;

        private bool _isSelected;

        public NodeViewModel(ModelNode modelNode)
        {
            Node = modelNode;
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
                OffsetX = value;
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(Coordinates));
            }
        }

        public double Y
        {
            get => Node.Coordinates.Y;
            set
            {
                Node.Coordinates.Y = value;
                OffsetY = value;
                OnPropertyChanged(nameof(Y));
                OnPropertyChanged(nameof(Coordinates));
            }
        }

        public double Z
        {
            get => Node.Coordinates.Z;
            set
            {
                Node.Coordinates.Z = value;
                OffsetZ = value;
                OnPropertyChanged(nameof(Z));
                OnPropertyChanged(nameof(Coordinates));
            }
        }

        public bool IsFixed
        {
            get => Node.IsFixed;
            set
            {
                Node.IsFixed = value;
                UpdateGeometryMesh();
                OnPropertyChanged(nameof(IsFixed));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                MaterialBrush = IsSelected ? SelectedNodeBrush : IsFixed ? FixedNodeBrush : FreeNodeBrush;
            }
        }

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

        protected override void UpdateGeometryMesh()
        {
            if (IsFixed)
            {
                GraphicsHelper.BuildPyramidMesh(MeshGeometry3D, 2);
            }
            else
            {
                GraphicsHelper.BuildCubeMesh(MeshGeometry3D, 1);
            }
        }
    }
}