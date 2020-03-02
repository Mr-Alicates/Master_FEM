using System.Windows.Media;
using System.Windows.Media.Media3D;
using POC3D.Helpers;

namespace POC3D.ViewModel
{
    public class ResultElementViewModel : GeometryViewModel
    {
        private static readonly Vector3D VerticalVector = new Vector3D(0, 0, 1);
        private static readonly Brush BarBrush = Brushes.Blue;

        public ResultElementViewModel(ResultNodeViewModel origin, ResultNodeViewModel destination)
        {
            Origin = origin;
            Destination = destination;
            UpdateGeometryMesh();
        }

        public ResultNodeViewModel Origin { get; }

        public ResultNodeViewModel Destination { get; }

        protected override void UpdateGeometryMesh()
        {
            if (Destination == null || Origin == null) return;

            var destinationCoordinates = Destination.NodeViewModel.Coordinates + Destination.Displacement;
            var originCoordinates = Origin.NodeViewModel.Coordinates + Origin.Displacement;

            var vector = destinationCoordinates - originCoordinates;

            GraphicsHelper.BuildBarMesh(MeshGeometry3D, vector.Length, 0.5);

            RotationAngle = Vector3D.AngleBetween(VerticalVector, vector);
            RotationAxis = Vector3D.CrossProduct(VerticalVector, vector);

            OffsetX = originCoordinates.X;
            OffsetY = originCoordinates.Y;
            OffsetZ = originCoordinates.Z;

            MaterialBrush = BarBrush;
        }
    }
}