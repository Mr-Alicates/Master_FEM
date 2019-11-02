using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Model;

namespace POC3D.ViewModel
{
    public class NodeViewModel : Observable
    {
        private static readonly Brush FreeNodeBrush = Brushes.LightGreen;
        private static readonly Brush FixedNodeBrush = Brushes.DarkGreen;
        private static readonly Brush SelectedNodeBrush = Brushes.Red;
        private MeshGeometry3D _meshGeometry3D;
        private DiffuseMaterial _material;

        private bool _isSelected;

        public NodeViewModel(ModelNode modelNode)
        {
            Node = modelNode;
            Geometry = BuildGeometry();
            UpdateGeometry();
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
                UpdateGeometry();
                OnPropertyChanged(nameof(X));
            }
        }
        public double Y
        {
            get => Node.Coordinates.Y;
            set
            {
                Node.Coordinates.Y = value;
                UpdateGeometry();
                OnPropertyChanged(nameof(Y));
            }
        }
        public double Z
        {
            get => Node.Coordinates.Z;
            set
            {
                Node.Coordinates.Z = value;
                UpdateGeometry();
                OnPropertyChanged(nameof(Z));
            }
        }

        public bool IsFixed
        {
            get => Node.IsFixed;
            set
            {
                Node.IsFixed = value;
                UpdateMaterial();
                UpdateGeometry();
                OnPropertyChanged(nameof(IsFixed));
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

        public GeometryModel3D Geometry { get; }
        
        public string Name => $"{Id} ({Coordinates.ToString()})";

        private GeometryModel3D BuildGeometry()
        {
            _meshGeometry3D = new MeshGeometry3D()
            {
                Positions = new Point3DCollection(),
                TriangleIndices = new Int32Collection()
            };

            _material = new DiffuseMaterial(FreeNodeBrush);

            return new GeometryModel3D()
            {
                Material = _material,
                Geometry = _meshGeometry3D
            };
        }

        private void UpdateMaterial()
        {
            _material.Brush = IsSelected ? SelectedNodeBrush : (IsFixed ? FixedNodeBrush : FreeNodeBrush);

            OnPropertyChanged(nameof(Geometry));
        }

        private void UpdateGeometry()
        {
            if (IsFixed)
            {
                SetGeometryAsPyramid();
            }
            else
            {
                SetGeometryAsCube();
            }
        }

        private void SetGeometryAsCube()
        {
            const int halfSize = 1;
            
            _meshGeometry3D.TriangleIndices.Clear();
            _meshGeometry3D.Positions.Clear();

            _meshGeometry3D.Positions = new Point3DCollection()
            {
                Coordinates + new Vector3D(-1, -1, -1) * halfSize,
                Coordinates + new Vector3D(1, -1, -1) * halfSize,
                Coordinates + new Vector3D(1, 1, -1) * halfSize,
                Coordinates + new Vector3D(-1, 1, -1) * halfSize,

                Coordinates + new Vector3D(-1, -1, 1) * halfSize,
                Coordinates + new Vector3D(1, -1, 1) * halfSize,
                Coordinates + new Vector3D(1, 1, 1) * halfSize,
                Coordinates + new Vector3D(-1, 1, 1) * halfSize,
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

            OnPropertyChanged(nameof(Geometry));
        }
        
        private void SetGeometryAsPyramid()
        {
            const int halfSize = 2;

            _meshGeometry3D.TriangleIndices.Clear();
            _meshGeometry3D.Positions.Clear();

            _meshGeometry3D.Positions = new Point3DCollection()
            {
                Coordinates + new Vector3D(-1, -1, -1) * halfSize,
                Coordinates + new Vector3D(1, -1, -1) * halfSize,
                Coordinates + new Vector3D(1, 1, -1) * halfSize,
                Coordinates + new Vector3D(-1, 1, -1) * halfSize,

                Coordinates + new Vector3D(0, 0, 1) * halfSize,
            };

            _meshGeometry3D.TriangleIndices = new Int32Collection()
            {
                //Bottom
                0,3,1,
                3,2,1,
                    
                //Left
                0,1,4,

                //Right
                1,2,4,

                //Back
                2,3,4,

                //Front
                3,0,4
            };


            OnPropertyChanged(nameof(Geometry));
        }
    }
}