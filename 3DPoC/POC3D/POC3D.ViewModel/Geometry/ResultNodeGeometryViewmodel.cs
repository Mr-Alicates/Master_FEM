using POC3D.ViewModel.Implementation;

namespace POC3D.ViewModel.Geometry
{
    public class ResultNodeGeometryViewModel : NodeGeometryViewModel
    {
        public ResultNodeGeometryViewModel(NodeViewModel nodeViewModel) 
            : base(nodeViewModel)
        {
        }

        protected override void UpdateGeometryMesh()
        {
            base.UpdateGeometryMesh();

            OffsetX += ViewModel.DisplacementX;
            OffsetY += ViewModel.DisplacementY;
            OffsetZ += ViewModel.DisplacementZ;
        }
    }
}
