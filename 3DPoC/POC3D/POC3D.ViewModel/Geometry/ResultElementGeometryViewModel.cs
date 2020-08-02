using POC3D.ViewModel.Configuration;
using POC3D.ViewModel.Implementation;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Geometry
{
    public class ResultElementGeometryViewModel : ElementGeometryViewModel
    {
        public ResultElementGeometryViewModel(ElementViewModel elementViewModel)
            : base(elementViewModel)
        {
        }
        
        protected override void UpdateGeometryMesh()
        {
            var destinationCoordinates = ViewModel.Destination.Coordinates + ViewModel.Destination.Displacement;
            var originCoordinates = ViewModel.Origin.Coordinates + ViewModel.Origin.Displacement;

            var vector = destinationCoordinates - originCoordinates;

            GraphicsHelper.BuildBarMesh(MeshGeometry3D, vector.Length, 0.5);

            RotationAngle = Vector3D.AngleBetween(VerticalVector, vector);
            RotationAxis = Vector3D.CrossProduct(VerticalVector, vector);

            OffsetX = originCoordinates.X;
            OffsetY = originCoordinates.Y;
            OffsetZ = originCoordinates.Z;

            MaterialBrush = ApplicationConfiguration.BarBrush;
        }
    }
}
