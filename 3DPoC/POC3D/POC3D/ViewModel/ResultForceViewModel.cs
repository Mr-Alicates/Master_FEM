using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class ResultForceViewModel
    {
        public ResultForceViewModel(ForceViewModel forceViewModel, ResultNodeViewModel resultNodeViewModel)
        {
            ForceViewModel = forceViewModel;
            ResultNodeViewModel = resultNodeViewModel;
            BuildGeometry();
        }

        public ForceViewModel ForceViewModel { get; }

        public ResultNodeViewModel ResultNodeViewModel { get; }

        public GeometryModel3D Geometry { get; private set; }

        private void BuildGeometry()
        {
            Geometry = new GeometryModel3D
            {
                Material = ForceViewModel.Geometry.Material,
                Geometry = ForceViewModel.Geometry.Geometry,
                Transform = new Transform3DGroup
                {
                    Children = new Transform3DCollection
                    {
                        ForceViewModel.Geometry.Transform,
                        ResultNodeViewModel.TranslateTransform3D
                    }
                }
            };
        }
    }
}