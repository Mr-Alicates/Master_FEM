using POC3D.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class ResultNodeViewModel
    {
        private TranslateTransform3D _translateTransform3D;

        public ResultNodeViewModel(NodeViewModel nodeViewModel)
        {
            NodeViewModel = nodeViewModel;
            BuildGeometry();
        }

        public NodeViewModel NodeViewModel { get; }

         public Vector3D Displacement => new Vector3D(
            DisplacementX,
            DisplacementY,
            DisplacementZ);

        public double DisplacementX
        {
            get => _translateTransform3D.OffsetX;
            set => _translateTransform3D.OffsetX = value;
        }

        public double DisplacementY
        {
            get => _translateTransform3D.OffsetY;
            set => _translateTransform3D.OffsetY = value;
        }

        public double DisplacementZ
        {
            get => _translateTransform3D.OffsetZ;
            set => _translateTransform3D.OffsetZ = value;
        }

        public GeometryModel3D Geometry { get; private set; }

        private void BuildGeometry()
        {
            _translateTransform3D = new TranslateTransform3D();

            Geometry = new GeometryModel3D
            {
                Material = NodeViewModel.Geometry.Material,
                Geometry = NodeViewModel.Geometry.Geometry,
                Transform = new Transform3DGroup
                {
                    Children = new Transform3DCollection
                    {
                        NodeViewModel.Geometry.Transform,
                        _translateTransform3D
                    }
                }
            };
        }
    }
}
