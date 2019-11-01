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

        public ElementViewModel(IModelElement modelElement)
        {
            Element = modelElement;
        }

        public bool IsSelected { get; set; }

        public IModelElement Element { get; }

        public NodeViewModel Origin => new NodeViewModel(Element.Nodes.First());

        public NodeViewModel Destination => new NodeViewModel(Element.Nodes.Last());

        public GeometryModel3D Geometry => BuildBar3D(Origin.Coordinates, Destination.Coordinates, IsSelected);

        private static GeometryModel3D BuildBar3D(Point3D origin, Point3D destination, bool isSelected)
        {
            var material = isSelected? SelectedBarBrush : BarBrush;
            const double halfSize = 0.5;
            
            GeometryModel3D result = new GeometryModel3D();
            result.Material = new DiffuseMaterial(material);

            var vector = destination - origin;
            var height = vector.Length;

            Point3D floor = new Point3D();
            Point3D roof = new Point3D(0, 0, height);

            MeshGeometry3D geometry = new MeshGeometry3D()
            {
                Positions = new Point3DCollection()
                {
                    floor + new Vector3D(-1, -1, 0) * halfSize,
                    floor + new Vector3D(1, -1, 0) * halfSize,
                    floor + new Vector3D(1, 1, 0) * halfSize,
                    floor + new Vector3D(-1, 1, 0) * halfSize,

                    roof + new Vector3D(-1, -1, 0) * halfSize,
                    roof + new Vector3D(1, -1, 0) * halfSize,
                    roof + new Vector3D(1, 1, 0) * halfSize,
                    roof + new Vector3D(-1, 1, 0) * halfSize,
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

            var verticalVector = new Vector3D(0, 0, 1);
            var rotationAngle = Vector3D.AngleBetween(verticalVector, vector);

            var rotationVector = Vector3D.CrossProduct(verticalVector, vector);
            
            result.Geometry = geometry;
            result.Transform = new Transform3DGroup()
            {
                Children = new Transform3DCollection()
                {
                    new RotateTransform3D(new AxisAngleRotation3D(rotationVector, rotationAngle)),
                    new TranslateTransform3D(origin.X, origin.Y, origin.Z)
                }
            };


            

            return result;
        }

        public string Name => $"({Origin.Id}) ---> ({Destination.Id})";
    }
}
