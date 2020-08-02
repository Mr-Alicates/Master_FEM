using POC3D.ViewModel.Configuration;
using POC3D.ViewModel.Implementation;
using System.Windows.Media;

namespace POC3D.ViewModel.Geometry
{
    public class NodeGeometryViewModel : GeometryViewModel<NodeViewModel>
    {
        public NodeGeometryViewModel(NodeViewModel nodeViewModel)
            : base(nodeViewModel)
        {
        }

        protected override void UpdateGeometryMesh()
        {
            OffsetX = ViewModel.X;
            OffsetY = ViewModel.Y;
            OffsetZ = ViewModel.Z;

            var isFixed = ViewModel.IsXFixed || ViewModel.IsYFixed || ViewModel.IsZFixed;

            MaterialBrush = ViewModel.IsSelected ? ApplicationConfiguration.SelectedNodeBrush : isFixed ? ApplicationConfiguration.FixedNodeBrush : ApplicationConfiguration.FreeNodeBrush;

            if (isFixed)
                GraphicsHelper.BuildPyramidMesh(MeshGeometry3D, 2);
            else
                GraphicsHelper.BuildCubeMesh(MeshGeometry3D, 1);
        }
    }
}
