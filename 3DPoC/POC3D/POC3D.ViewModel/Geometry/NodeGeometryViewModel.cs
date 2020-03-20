using System.Windows.Media;

namespace POC3D.ViewModel.Geometry
{
    public class NodeGeometryViewModel : GeometryViewModel<NodeViewModel>
    {
        private static readonly Brush FreeNodeBrush = Brushes.LightGreen;
        private static readonly Brush FixedNodeBrush = Brushes.DarkGreen;
        private static readonly Brush SelectedNodeBrush = Brushes.Red;

        public NodeGeometryViewModel(NodeViewModel nodeViewModel)
            : base(nodeViewModel)
        {
        }

        protected override void UpdateGeometryMesh()
        {
            OffsetX = ViewModel.X;
            OffsetY = ViewModel.Y;
            OffsetZ = ViewModel.Z;

            MaterialBrush = ViewModel.IsSelected ? SelectedNodeBrush : ViewModel.IsFixed ? FixedNodeBrush : FreeNodeBrush;

            if (ViewModel.IsFixed)
                GraphicsHelper.BuildPyramidMesh(MeshGeometry3D, 2);
            else
                GraphicsHelper.BuildCubeMesh(MeshGeometry3D, 1);
        }
    }
}
