using POC3D.ViewModel.Configuration;
using POC3D.ViewModel.Implementation;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Geometry
{
    public class ForceGeometryViewModel : GeometryViewModel<ForceViewModel>
    {
        private static readonly Vector3D VerticalVector = new Vector3D(0, 0, 1);

        public ForceGeometryViewModel(ForceViewModel forceGeometryViewModel)
            : base(forceGeometryViewModel)
        {
        }

        protected override void UpdateGeometryMesh()
        {
            RotationAngle = Vector3D.AngleBetween(VerticalVector, -ViewModel.ApplicationVector);
            RotationAxis = Vector3D.CrossProduct(VerticalVector, -ViewModel.ApplicationVector);

            MaterialBrush = ViewModel.IsSelected ? ApplicationConfiguration.SelectedForceBrush : ApplicationConfiguration.ForceBrush;

            GraphicsHelper.BuildForceArrow(MeshGeometry3D, 10, 2);

            OffsetX = ViewModel.Node.X;
            OffsetY = ViewModel.Node.Y;
            OffsetZ = ViewModel.Node.Z;
        }
    }
}
