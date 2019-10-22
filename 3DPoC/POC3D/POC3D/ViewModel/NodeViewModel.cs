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
                OnPropertyChanged(nameof(X));
            }
        }
        public double Y
        {
            get => Node.Coordinates.Y;
            set
            {
                Node.Coordinates.Y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
        public double Z
        {
            get => Node.Coordinates.Z;
            set
            {
                Node.Coordinates.Z = value;
                OnPropertyChanged(nameof(Z));
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

        public bool IsSelected { get; set; }

        public NodeViewModel SetAsFixed()
        {
            Node.SetAsFixed();
            return this;
        }

        public NodeViewModel SetAsFree()
        {
            Node.SetAsFree();
            return this;
        }

        public GeometryModel3D Geometry
        {
            get
            {
                if (IsFixed)
                {
                    var brush = IsSelected ? SelectedNodeBrush : FixedNodeBrush;

                    return BuildPyramid3D(Coordinates, brush);
                }
                else
                {
                    var brush = IsSelected ? SelectedNodeBrush : FreeNodeBrush;

                    return BuildCube3D(Coordinates, brush);
                }
            }
        }

        private static GeometryModel3D BuildCube3D(Point3D center, Brush material)
        {
            const int halfSize = 1;

            GeometryModel3D result = new GeometryModel3D();
            result.Material = new DiffuseMaterial(material);

            result.Geometry = new MeshGeometry3D()
            {
                Positions = new Point3DCollection()
                {
                    center + new Vector3D(-1, -1, -1) * halfSize,
                    center + new Vector3D(1, -1, -1) * halfSize,
                    center + new Vector3D(1, 1, -1) * halfSize,
                    center + new Vector3D(-1, 1, -1) * halfSize,

                    center + new Vector3D(-1, -1, 1) * halfSize,
                    center + new Vector3D(1, -1, 1) * halfSize,
                    center + new Vector3D(1, 1, 1) * halfSize,
                    center + new Vector3D(-1, 1, 1) * halfSize,
                },
                TriangleIndices = new Int32Collection()
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
                }
            };


            return result;
        }

        private static GeometryModel3D BuildPyramid3D(Point3D center, Brush material)
        {
            const int halfSize = 2;

            GeometryModel3D result = new GeometryModel3D();
            result.Material = new DiffuseMaterial(material);

            result.Geometry = new MeshGeometry3D()
            {
                Positions = new Point3DCollection()
                {
                    center + new Vector3D(-1, -1, -1) * halfSize,
                    center + new Vector3D(1, -1, -1) * halfSize,
                    center + new Vector3D(1, 1, -1) * halfSize,
                    center + new Vector3D(-1, 1, -1) * halfSize,

                    center + new Vector3D(0, 0, 1) * halfSize,
                },
                TriangleIndices = new Int32Collection()
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
                }
            };


            return result;
        }

        public string Name => $"{Id} ({Coordinates.ToString()})";
    }
}