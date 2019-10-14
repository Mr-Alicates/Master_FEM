﻿using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Model;

namespace POC3D.ViewModel
{
    public class NodeViewModel
    {
        private readonly ModelNode _modelNode;

        public NodeViewModel(ModelNode modelNode)
        {
            _modelNode = modelNode;
        }

        //To be deleted...
        public ModelNode Node => _modelNode;
        
        public Point3D Coordinates => new Point3D(
            _modelNode.Coordinates.X,
            _modelNode.Coordinates.Y,
            _modelNode.Coordinates.Z);

        public bool IsFixed => _modelNode.IsFixed;

        public NodeViewModel SetAsFixed()
        {
            _modelNode.SetAsFixed();
            return this;
        }

        public NodeViewModel SetAsFree()
        {
            _modelNode.SetAsFree();
            return this;
        }

        public GeometryModel3D Geometry => BuildCube3D(Coordinates, IsFixed);

        private static GeometryModel3D BuildCube3D(Point3D center, bool isFixed)
        {
            var material = isFixed ? Brushes.Red : Brushes.Green;
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
    }
}