using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            mesh.Positions = new Point3DCollection()
            {
                new Point3D(-halfSize, -halfSize, -halfSize),
                new Point3D(halfSize, -halfSize, -halfSize),
                new Point3D(halfSize, halfSize, -halfSize),
                new Point3D(-halfSize, halfSize, -halfSize),
                new Point3D(-halfSize, -halfSize, halfSize),
                new Point3D(halfSize, -halfSize, halfSize),
                new Point3D(halfSize, halfSize, halfSize),
                new Point3D(-halfSize, halfSize, halfSize),
            };

            mesh.TriangleIndices = new Int32Collection()
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
        }

        public static void BuildPyramidMesh(MeshGeometry3D mesh, double size)
        {
            var halfSize = size / 2.0;

            mesh.TriangleIndices.Clear();
            mesh.Positions.Clear();

            mesh.Positions = new Point3DCollection()
            {
                new Point3D(-halfSize, -halfSize, -halfSize),
                new Point3D(halfSize, -halfSize, -halfSize),
                new Point3D(halfSize, halfSize, -halfSize),
                new Point3D(-halfSize, halfSize, -halfSize),
                new Point3D(0, 0, halfSize),
            };

            mesh.TriangleIndices = new Int32Collection()
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
        }

        public static void BuildBarMesh(MeshGeometry3D mesh, double height, double sectionWidth)
        {
            var halfSize = sectionWidth / 2.0;

            mesh.TriangleIndices.Clear();
            mesh.Positions.Clear();

            mesh.Positions = new Point3DCollection()
            {
                new Point3D(-halfSize, -halfSize, 0),
                new Point3D(halfSize, -halfSize, 0),
                new Point3D(halfSize, halfSize, 0),
                new Point3D(-halfSize, halfSize, 0),
                new Point3D(-halfSize, -halfSize, height),
                new Point3D(halfSize, -halfSize, height),
                new Point3D(halfSize, halfSize, height),
                new Point3D(-halfSize, halfSize, height),
            };

            mesh.TriangleIndices = new Int32Collection()
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
        }
    }
}
