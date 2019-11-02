using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
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

        public ElementViewModel(IModelElement modelElement, NodeViewModel origin, NodeViewModel destination)
        {
            Element = modelElement;
            Origin = origin;
            Destination = destination;

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
                UpdateMaterial();
            }
        }

        public IModelElement Element { get; }

        public NodeViewModel Origin { get; }

        public NodeViewModel Destination { get; }

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

        private void UpdateMaterial()
        {
            _material.Brush = IsSelected ? SelectedBarBrush : BarBrush;

            OnPropertyChanged(nameof(Geometry));
        }

        private void UpdateGeometry()
        {
            const double halfSize = 0.5;

            _meshGeometry3D.TriangleIndices.Clear();
            _meshGeometry3D.Positions.Clear();

            var vector = (Destination.Coordinates - Origin.Coordinates);
            var height = vector.Length;

            Point3D floor = new Point3D();
            Point3D roof = new Point3D(0, 0, height);

            _meshGeometry3D.Positions = new Point3DCollection()
            {
                floor + new Vector3D(-1, -1, 0) * halfSize,
                floor + new Vector3D(1, -1, 0) * halfSize,
                floor + new Vector3D(1, 1, 0) * halfSize,
                floor + new Vector3D(-1, 1, 0) * halfSize,

                roof + new Vector3D(-1, -1, 0) * halfSize,
                roof + new Vector3D(1, -1, 0) * halfSize,
                roof + new Vector3D(1, 1, 0) * halfSize,
                roof + new Vector3D(-1, 1, 0) * halfSize,
            };

            _meshGeometry3D.TriangleIndices = new Int32Collection()
            {
                //Bottom
                0,3,1,
                3,2,1,

                //Top
                7,4,5,
                7,5,6,

                //Left
                0,5,4,
                0,1,5,

                //Right
                7,6,3,
                3,6,2,

                //Back
                1,2,5,
                5,2,6,

                //Front
                7,3,4,
                4,3,0,
            };


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
