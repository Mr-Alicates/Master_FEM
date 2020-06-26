using POC3D.ViewModel.Configuration;
using POC3D.ViewModel.Implementation;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Geometry
{
    public class ElementGeometryViewModel : GeometryViewModel<ElementViewModel>
    {
        protected static readonly Vector3D VerticalVector = new Vector3D(0, 0, 1);

        public ElementGeometryViewModel(ElementViewModel elementViewModel)
            : base(elementViewModel)
        {
        }
        
        protected override void UpdateGeometryMesh()
        {
            MaterialBrush = ViewModel.IsSelected ? ApplicationConfiguration.SelectedBarBrush : ApplicationConfiguration.BarBrush;

            var vector = ViewModel.Destination.Coordinates - ViewModel.Origin.Coordinates;

            var length = vector.Length;

            if(VerticalVector == -vector)
            {
                //This is a hack: When the vector is opposite to vertical the bar is built in the wrong direction
                length = -length;
            }

            GraphicsHelper.BuildBarMesh(MeshGeometry3D, length, 0.5);

            RotationAngle = Vector3D.AngleBetween(VerticalVector, vector);
            RotationAxis = Vector3D.CrossProduct(VerticalVector, vector);

            OffsetX = ViewModel.Origin.X;
            OffsetY = ViewModel.Origin.Y;
            OffsetZ = ViewModel.Origin.Z;
        }
    }
}
