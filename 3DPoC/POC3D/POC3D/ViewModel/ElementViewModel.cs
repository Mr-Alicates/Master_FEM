using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Helpers;
using POC3D.Model;

namespace POC3D.ViewModel
{
    public class ElementViewModel : Observable
    {
        private static readonly Brush BarBrush = Brushes.Blue;
        private static readonly Brush SelectedBarBrush = Brushes.Red;
        private MeshGeometry3D _meshGeometry3D;
        private DiffuseMaterial _material;
        private bool _isSelected;
        private NodeViewModel _origin;
        private NodeViewModel _destination;

        public ElementViewModel(IModelElement modelElement, NodeViewModel origin, NodeViewModel destination)
        {
            Element = modelElement;
            _origin = origin;
            _destination = destination;

            Geometry = BuildGeometry();
            UpdateGeometry();

            Origin.PropertyChanged += NodesChanged;
            Destination.PropertyChanged += NodesChanged;
        }

        private void NodesChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Origin.Geometry))
            {
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

        public string Name => $"({Origin.Id}) ---> ({Destination.Id})";

        public GeometryModel3D Geometry { get; }

        private GeometryModel3D BuildGeometry()
        {
            _meshGeometry3D = new MeshGeometry3D()
            {
                Positions = new Point3DCollection(),
                TriangleIndices = new Int32Collection()
            };

            _material = new DiffuseMaterial(BarBrush);

            return new GeometryModel3D()
            {
                Material = _material,
                Geometry = _meshGeometry3D
            };
        }
        
        private void UpdateGeometry()
        {
            _material.Brush = IsSelected ? SelectedBarBrush : BarBrush;

            var vector = (Destination.Coordinates - Origin.Coordinates);

            GraphicsHelper.BuildBarMesh(_meshGeometry3D, vector.Length, 0.5);

            var verticalVector = new Vector3D(0, 0, 1);
            var rotationAngle = Vector3D.AngleBetween(verticalVector, vector);
            var rotationVector = Vector3D.CrossProduct(verticalVector, vector);

            Geometry.Transform = new Transform3DGroup()
            {
                Children = new Transform3DCollection()
                {
                    new RotateTransform3D(new AxisAngleRotation3D(rotationVector, rotationAngle)),
                    new TranslateTransform3D(Origin.X, Origin.Y, Origin.Z)
                }
            };

            OnPropertyChanged(nameof(Geometry));
        }
    }
}
