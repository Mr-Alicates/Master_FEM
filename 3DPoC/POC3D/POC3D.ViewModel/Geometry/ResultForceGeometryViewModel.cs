using POC3D.ViewModel.Implementation;

namespace POC3D.ViewModel.Geometry
{
    public class ResultForceGeometryViewModel : ForceGeometryViewModel
    {
        public ResultForceGeometryViewModel(ForceViewModel forceViewModel)
            : base(forceViewModel)
        {
        }

        protected override void UpdateGeometryMesh()
        {
            base.UpdateGeometryMesh();

            OffsetX += ViewModel.Node.DisplacementX;
            OffsetY += ViewModel.Node.DisplacementY;
            OffsetZ += ViewModel.Node.DisplacementZ;
        }
    }
}
