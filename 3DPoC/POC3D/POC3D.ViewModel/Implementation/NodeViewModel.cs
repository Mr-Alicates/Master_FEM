using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Geometry;

namespace POC3D.ViewModel.Implementation
{
    public class NodeViewModel : SelectableViewModel
    {
        private NodeGeometryViewModel _nodeGeometryViewModel;
        private ResultNodeGeometryViewModel _resultNodeGeometryViewmodel;

        private double _displacementX;
        private double _displacementY;
        private double _displacementZ;

        public NodeViewModel(IModelNode modelNode)
        {
            Node = modelNode;
            _nodeGeometryViewModel = new NodeGeometryViewModel(this);
            _resultNodeGeometryViewmodel = new ResultNodeGeometryViewModel(this);
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
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Geometry));
                OnPropertyChanged(nameof(ResultGeometry));
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
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Geometry));
                OnPropertyChanged(nameof(ResultGeometry));
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
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Geometry));
                OnPropertyChanged(nameof(ResultGeometry));
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

        public Vector3D Displacement => new Vector3D(
            DisplacementX,
            DisplacementY,
            DisplacementZ);

        public double DisplacementX
        {
            get => _displacementX;
            set
            {
                _displacementX = value;
                OnPropertyChanged(nameof(DisplacementX));
                OnPropertyChanged(nameof(Displacement));
                OnPropertyChanged(nameof(ResultGeometry));
            }
        }

        public double DisplacementY
        {
            get => _displacementY;
            set
            {
                _displacementY = value;
                OnPropertyChanged(nameof(DisplacementY));
                OnPropertyChanged(nameof(Displacement));
                OnPropertyChanged(nameof(ResultGeometry));
            }
        }

        public double DisplacementZ
        {
            get => _displacementZ;
            set
            {
                _displacementZ = value;
                OnPropertyChanged(nameof(DisplacementZ));
                OnPropertyChanged(nameof(Displacement));
                OnPropertyChanged(nameof(ResultGeometry));
            }
        }

        public string Name => $"{Id} ({Coordinates.ToString()})";

        public GeometryModel3D Geometry => _nodeGeometryViewModel.Geometry;

        public GeometryModel3D ResultGeometry => _resultNodeGeometryViewmodel.Geometry;
    }
}