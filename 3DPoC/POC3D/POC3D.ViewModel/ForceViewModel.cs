using System;
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
        private readonly ResultForceGeometryViewModel _resultForceGeometryViewModel;

        public ForceViewModel(IModelForce force, NodeViewModel node)
        {
            Force = force;

            _nodeViewModel = node;
            _nodeViewModel.PropertyChanged += NodeChanged;

            _forceGeometryViewModel = new ForceGeometryViewModel(this);
            _resultForceGeometryViewModel = new ResultForceGeometryViewModel(this);
        }

        public IModelForce Force { get; }

        public NodeViewModel Node
        {
            get => _nodeViewModel;
            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException(nameof(Node));
                }

                _nodeViewModel.PropertyChanged -= NodeChanged;
                _nodeViewModel = value;
                _nodeViewModel.PropertyChanged += NodeChanged;

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
                OnPropertyChanged(nameof(Name));
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
                OnPropertyChanged(nameof(Name));
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
                OnPropertyChanged(nameof(Name));
            }
        }

        public Vector3D ApplicationVector => new Vector3D(Force.ApplicationVector.X, Force.ApplicationVector.Y,
            Force.ApplicationVector.Z);

        public double Magnitude => Force.Magnitude;

        public string Name =>
            $"({Node.Id}) ---> ({ApplicationVectorX:N}/{ApplicationVectorY:N}/{ApplicationVectorZ:N}) ({Magnitude:N})";

        public GeometryModel3D Geometry => _forceGeometryViewModel.Geometry;

        public GeometryModel3D ResultGeometry => _resultForceGeometryViewModel.Geometry;

        private void NodeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(NodeViewModel.Coordinates))
            {
                OnPropertyChanged(nameof(Node));
            }
        }
    }
}