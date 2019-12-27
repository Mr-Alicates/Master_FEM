using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace POC3D.Helpers
{
    public static class GraphicsHelper
    {
        public static void BuildCubeMesh(MeshGeometry3D mesh, double size)
        {
            var halfSize = size / 2.0;

            mesh.TriangleIndices.Clear();
            mesh.Positions.Clear();

            mesh.Positions = new Point3DCollection
            {
                new Point3D(-halfSize, -halfSize, -halfSize),
                new Point3D(halfSize, -halfSize, -halfSize),
                new Point3D(halfSize, halfSize, -halfSize),
                new Point3D(-halfSize, halfSize, -halfSize),
                new Point3D(-halfSize, -halfSize, halfSize),
                new Point3D(halfSize, -halfSize, halfSize),
                new Point3D(halfSize, halfSize, halfSize),
                new Point3D(-halfSize, halfSize, halfSize)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                //Bottom
                0, 3, 1,
                3, 2, 1,

                //Top
                7, 4, 5,
                7, 5, 6,

                //Left
                0, 5, 4,
                0, 1, 5,

                //Right
                7, 6, 3,
                3, 6, 2,

                //Back
                1, 2, 5,
                5, 2, 6,

                //Front
                7, 3, 4,
                4, 3, 0
            };
        }

        public static void BuildPyramidMesh(MeshGeometry3D mesh, double size)
        {
            var halfSize = size / 2.0;

            mesh.TriangleIndices.Clear();
            mesh.Positions.Clear();

            mesh.Positions = new Point3DCollection
            {
                new Point3D(-halfSize, -halfSize, -halfSize),
                new Point3D(halfSize, -halfSize, -halfSize),
                new Point3D(halfSize, halfSize, -halfSize),
                new Point3D(-halfSize, halfSize, -halfSize),
                new Point3D(0, 0, halfSize)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                //Bottom
                0, 3, 1,
                3, 2, 1,

                //Left
                0, 1, 4,

                //Right
                1, 2, 4,

                //Back
                2, 3, 4,

                //Front
                3, 0, 4
            };
        }

        public static void BuildBarMesh(MeshGeometry3D mesh, double height, double sectionWidth)
        {
            var halfSize = sectionWidth / 2.0;

            mesh.TriangleIndices.Clear();
            mesh.Positions.Clear();

            mesh.Positions = new Point3DCollection
            {
                new Point3D(-halfSize, -halfSize, 0),
                new Point3D(halfSize, -halfSize, 0),
                new Point3D(halfSize, halfSize, 0),
                new Point3D(-halfSize, halfSize, 0),
                new Point3D(-halfSize, -halfSize, height),
                new Point3D(halfSize, -halfSize, height),
                new Point3D(halfSize, halfSize, height),
                new Point3D(-halfSize, halfSize, height)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                //Bottom
                0, 3, 1,
                3, 2, 1,

                //Top
                7, 4, 5,
                7, 5, 6,

                //Left
                0, 5, 4,
                0, 1, 5,

                //Right
                7, 6, 3,
                3, 6, 2,

                //Back
                1, 2, 5,
                5, 2, 6,

                //Front
                7, 3, 4,
                4, 3, 0
            };
        }

        public static Model3DGroup BuildOrigin()
        {
            var zArrow = new GeometryModel3D
            {
                Geometry = BuildArrow(),
                Material = new MaterialGroup
                {
                    Children = new MaterialCollection
                    {
                        new DiffuseMaterial(Brushes.Green),
                        new EmissiveMaterial(Brushes.Green)
                    }
                }
            };

            var yArrow = new GeometryModel3D
            {
                Geometry = BuildArrow(),
                Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(-1, 0, 0), 90)),
                Material = new MaterialGroup
                {
                    Children = new MaterialCollection
                    {
                        new DiffuseMaterial(Brushes.Blue),
                        new EmissiveMaterial(Brushes.Blue)
                    }
                }
            };

            var xArrow = new GeometryModel3D
            {
                Geometry = BuildArrow(),
                Transform = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 90)),
                Material = new MaterialGroup
                {
                    Children = new MaterialCollection
                    {
                        new DiffuseMaterial(Brushes.Red),
                        new EmissiveMaterial(Brushes.Red)
                    }
                }
            };

            var cubeMesh = new MeshGeometry3D();
            BuildCubeMesh(cubeMesh, 0.5);

            var center = new GeometryModel3D
            {
                Geometry = cubeMesh,
                Material = new MaterialGroup
                {
                    Children = new MaterialCollection
                    {
                        new DiffuseMaterial(Brushes.Black),
                        new EmissiveMaterial(Brushes.Black)
                    }
                }
            };

            var result = new Model3DGroup
            {
                Children = new Model3DCollection
                {
                    zArrow,
                    yArrow,
                    xArrow,
                    center
                }
            };

            return result;
        }

        public static void BuildForceArrow(MeshGeometry3D mesh, double length, double width)
        {
            var halfSize = width / 2;
            var tenthSize = width / 10;

            mesh.TriangleIndices.Clear();
            mesh.Positions.Clear();

            mesh.Positions = new Point3DCollection
            {
                new Point3D(-halfSize, -halfSize, width),
                new Point3D(halfSize, -halfSize, width),
                new Point3D(halfSize, halfSize, width),
                new Point3D(-halfSize, halfSize, width),
                new Point3D(0, 0, 0),

                new Point3D(-tenthSize, -tenthSize, width),
                new Point3D(tenthSize, -tenthSize, width),
                new Point3D(tenthSize, tenthSize, width),
                new Point3D(-tenthSize, tenthSize, width),
                new Point3D(0, 0, length)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                //Bottom
                0, 3, 1,
                3, 2, 1,

                //Left
                0, 1, 4,

                //Right
                1, 2, 4,

                //Back
                2, 3, 4,

                //Front
                3, 0, 4,

                //Stem
                6, 5, 9,
                7, 6, 9,
                8, 7, 9,
                5, 8, 9
            };
        }

        public static MeshGeometry3D BuildArrow()
        {
            var mesh = new MeshGeometry3D();

            var halfSize = 0.5;
            var tenthSize = 0.05;

            var length = 10;

            mesh.TriangleIndices.Clear();
            mesh.Positions.Clear();

            mesh.Positions = new Point3DCollection
            {
                new Point3D(-halfSize, -halfSize, length - halfSize),
                new Point3D(halfSize, -halfSize, length - halfSize),
                new Point3D(halfSize, halfSize, length - halfSize),
                new Point3D(-halfSize, halfSize, length - halfSize),
                new Point3D(0, 0, length),

                new Point3D(-tenthSize, -tenthSize, length - halfSize),
                new Point3D(tenthSize, -tenthSize, length - halfSize),
                new Point3D(tenthSize, tenthSize, length - halfSize),
                new Point3D(-tenthSize, tenthSize, length - halfSize),
                new Point3D(0, 0, 0)
            };

            mesh.TriangleIndices = new Int32Collection
            {
                //Bottom
                0, 3, 1,
                3, 2, 1,

                //Left
                0, 1, 4,

                //Right
                1, 2, 4,

                //Back
                2, 3, 4,

                //Front
                3, 0, 4,

                //Stem
                6, 5, 9,
                7, 6, 9,
                8, 7, 9,
                5, 8, 9
            };

            return mesh;
        }
    }
}