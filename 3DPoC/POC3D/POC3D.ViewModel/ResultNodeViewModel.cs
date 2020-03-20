using POC3D.ViewModel.Base;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class ResultNodeViewModel : Observable
    {
        public ResultNodeViewModel(NodeViewModel nodeViewModel)
        {
            NodeViewModel = nodeViewModel;
            BuildGeometry();
        }

        public NodeViewModel NodeViewModel { get; }

        public TranslateTransform3D TranslateTransform3D { get; private set; }

        public Vector3D Displacement => new Vector3D(
            DisplacementX,
            DisplacementY,
            DisplacementZ);

        public double DisplacementX
        {
            get => TranslateTransform3D.OffsetX;
            set => TranslateTransform3D.OffsetX = value;
        }

        public double DisplacementY
        {
            get => TranslateTransform3D.OffsetY;
            set => TranslateTransform3D.OffsetY = value;
        }

        public double DisplacementZ
        {
            get => TranslateTransform3D.OffsetZ;
            set => TranslateTransform3D.OffsetZ = value;
        }

        public GeometryModel3D Geometry { get; private set; }

        private void BuildGeometry()
        {
            TranslateTransform3D = new TranslateTransform3D();

            Geometry = new GeometryModel3D
            {
                Material = NodeViewModel.Geometry.Material,
                Geometry = NodeViewModel.Geometry.Geometry,
                Transform = new Transform3DGroup
                {
                    Children = new Transform3DCollection
                    {
                        NodeViewModel.Geometry.Transform,
                        TranslateTransform3D
                    }
                }
            };
        }
    }
}