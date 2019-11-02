using System;
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
                UpdateGeometry();
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

        private void UpdateGeometry()
        {
            if (IsFixed)
            {
                GraphicsHelper.BuildPyramidMesh(_meshGeometry3D, 4);
            }
            else
            {
                GraphicsHelper.BuildCubeMesh(_meshGeometry3D, 2);
            }

            Geometry.Transform = new TranslateTransform3D(X, Y, Z);
            _material.Brush = IsSelected ? SelectedNodeBrush : (IsFixed ? FixedNodeBrush : FreeNodeBrush);

            OnPropertyChanged(nameof(Geometry));
        }
    }
}