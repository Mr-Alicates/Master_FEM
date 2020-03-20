using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel.Geometry
{
    public class ElementGeometryViewModel : GeometryViewModel<ElementViewModel>
    {
        private static readonly Vector3D VerticalVector = new Vector3D(0, 0, 1);
        private static readonly Brush BarBrush = Brushes.Blue;
        private static readonly Brush SelectedBarBrush = Brushes.Red;

        public ElementGeometryViewModel(ElementViewModel elementViewModel)
            : base(elementViewModel)
        {
        }
        
        protected override void UpdateGeometryMesh()
        {
            MaterialBrush = ViewModel.IsSelected ? SelectedBarBrush : BarBrush;

            var vector = ViewModel.Destination.Coordinates - ViewModel.Origin.Coordinates;

            GraphicsHelper.BuildBarMesh(MeshGeometry3D, vector.Length, 0.5);

            RotationAngle = Vector3D.AngleBetween(VerticalVector, vector);
            RotationAxis = Vector3D.CrossProduct(VerticalVector, vector);

            OffsetX = ViewModel.Origin.X;
            OffsetY = ViewModel.Origin.Y;
            OffsetZ = ViewModel.Origin.Z;
        }
    }
}
