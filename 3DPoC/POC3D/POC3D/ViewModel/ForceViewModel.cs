using POC3D.Helpers;
using POC3D.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class ForceViewModel : Observable
    {
        private static readonly Brush ForceBrush = Brushes.Yellow;
        private static readonly Brush SelectedForceBrush = Brushes.Red;
        private MeshGeometry3D _meshGeometry3D;
        private DiffuseMaterial _material;
        private bool _isSelected;
        private NodeViewModel _nodeViewModel;

        public ForceViewModel(ModelForce force, NodeViewModel node)
        {
            Force = force;

            _nodeViewModel = node;

            Geometry = BuildGeometry();
            UpdateGeometry();


            Node.PropertyChanged += NodesChanged;
        }
        
        public ModelForce Force { get; }

        public NodeViewModel Node
        {
            get => _nodeViewModel;
            set
            {
                _nodeViewModel = value;

                Force.Node = _nodeViewModel.Node;
                UpdateGeometry();
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                UpdateGeometry();
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public double ApplicationVectorX
        {
            get => Force.ApplicationVector.X;
            set
            {
                Force.ApplicationVector.X = value;
                UpdateGeometry();
                OnPropertyChanged(nameof(ApplicationVectorX));
            }
        }
        public double ApplicationVectorY
        {
            get => Force.ApplicationVector.Y;
            set
            {
                Force.ApplicationVector.Y = value;
                UpdateGeometry();
                OnPropertyChanged(nameof(ApplicationVectorY));
            }
        }
        public double ApplicationVectorZ
        {
            get => Force.ApplicationVector.Z;
            set
            {
                Force.ApplicationVector.Z = value;
                UpdateGeometry();
                OnPropertyChanged(nameof(ApplicationVectorZ));
            }
        }

        public Vector3D ApplicationVector => new Vector3D(ApplicationVectorX, ApplicationVectorY, ApplicationVectorZ);

        public double Magnitude
        {
            get => Force.Magnitude;
            set => Force.Magnitude = value;
        }

        public GeometryModel3D Geometry { get; }

        public string Name => $"({Node.Id}) ---> ({ApplicationVectorX:N}/{ApplicationVectorY:N}/{ApplicationVectorZ:N}) ({Magnitude:N})";

        private void NodesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Node.Geometry))
            {
                UpdateGeometry();
            }
        }

        private GeometryModel3D BuildGeometry()
        {
            _meshGeometry3D = new MeshGeometry3D()
            {
                Positions = new Point3DCollection(),
                TriangleIndices = new Int32Collection()
            };

            _material = new DiffuseMaterial(ForceBrush);

            return new GeometryModel3D()
            {
                Material = _material,
                Geometry = _meshGeometry3D
            };
        }

        private void UpdateGeometry()
        {
            _material.Brush = IsSelected ? SelectedForceBrush : ForceBrush;

            GraphicsHelper.BuildForceArrow(_meshGeometry3D, 10, 2);

            var verticalVector = new Vector3D(0, 0, 1);
            var rotationAngle = Vector3D.AngleBetween(verticalVector, -ApplicationVector);
            var rotationVector = Vector3D.CrossProduct(verticalVector, -ApplicationVector);

            Geometry.Transform = new Transform3DGroup()
            {
                Children = new Transform3DCollection()
                {
                    new RotateTransform3D(new AxisAngleRotation3D(rotationVector, rotationAngle)),
                    new TranslateTransform3D(Node.X, Node.Y, Node.Z)
                }
            };

            OnPropertyChanged(nameof(Geometry));
        }
    }
}
