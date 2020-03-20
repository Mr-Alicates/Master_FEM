using System.Windows.Media.Media3D;
using POC3D.Model;
using POC3D.ViewModel.Base;
using POC3D.ViewModel.Geometry;

namespace POC3D.ViewModel
{
    public class ForceViewModel : SelectableViewModel
    {
        private NodeViewModel _nodeViewModel;
        private readonly ForceGeometryViewModel _forceGeometryViewModel;

        public ForceViewModel(IModelForce force, NodeViewModel node)
        {
            Force = force;
            Node = node;
            _forceGeometryViewModel = new ForceGeometryViewModel(this);
        }

        public IModelForce Force { get; }

        public NodeViewModel Node
        {
            get => _nodeViewModel;
            set
            {
                if (_nodeViewModel != null) _nodeViewModel.PropertyChanged -= (_, __) => OnPropertyChanged(nameof(Node)); ;

                value.PropertyChanged += (_, __) => OnPropertyChanged(nameof(Node)); ;
                _nodeViewModel = value;
                Force.Node = _nodeViewModel.Node;
                OnPropertyChanged(nameof(Node));
            }
        }

        public double ApplicationVectorX
        {
            get => Force.ApplicationVector.X;
            set
            {
                Force.ApplicationVector.X = value;
                OnPropertyChanged(nameof(ApplicationVectorX));
                OnPropertyChanged(nameof(ApplicationVector));
                OnPropertyChanged(nameof(Magnitude));
            }
        }

        public double ApplicationVectorY
        {
            get => Force.ApplicationVector.Y;
            set
            {
                Force.ApplicationVector.Y = value;
                OnPropertyChanged(nameof(ApplicationVectorY));
                OnPropertyChanged(nameof(ApplicationVector));
                OnPropertyChanged(nameof(Magnitude));
            }
        }

        public double ApplicationVectorZ
        {
            get => Force.ApplicationVector.Z;
            set
            {
                Force.ApplicationVector.Z = value;
                OnPropertyChanged(nameof(ApplicationVectorZ));
                OnPropertyChanged(nameof(ApplicationVector));
                OnPropertyChanged(nameof(Magnitude));
            }
        }

        public Vector3D ApplicationVector => new Vector3D(Force.ApplicationVector.X, Force.ApplicationVector.Y,
            Force.ApplicationVector.Z);

        public double Magnitude => Force.Magnitude;

        public string Name =>
            $"({Node.Id}) ---> ({ApplicationVectorX:N}/{ApplicationVectorY:N}/{ApplicationVectorZ:N}) ({Magnitude:N})";

        public GeometryModel3D Geometry => _forceGeometryViewModel.Geometry;
    }
}