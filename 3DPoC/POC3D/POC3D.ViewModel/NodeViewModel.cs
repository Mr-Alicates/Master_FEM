using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Geometry;

namespace POC3D.ViewModel
{
    public class NodeViewModel : SelectableViewModel
    {
        private NodeGeometryViewModel _nodeGeometryViewModel;

        public NodeViewModel(IModelNode modelNode)
        {
            Node = modelNode;
            _nodeGeometryViewModel = new NodeGeometryViewModel(this);
        }

        public int Id => Node.Id;

        public IModelNode Node { get; }

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
                OnPropertyChanged(nameof(IsFixed));
            }
        }

        public string Name => $"{Id} ({Coordinates.ToString()})";

        public GeometryModel3D Geometry => _nodeGeometryViewModel.Geometry;
    }
}