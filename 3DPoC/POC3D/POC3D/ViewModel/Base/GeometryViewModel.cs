using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public abstract class GeometryViewModel : Observable
    {
        private DiffuseMaterial _material;
        private TranslateTransform3D _translateTransform3D;
        
        protected GeometryViewModel()
        {
            BuildGeometry();
        }

        protected double OffsetX
        {
            get => _translateTransform3D.OffsetX;
            set => _translateTransform3D.OffsetX = value;
        }

        protected double OffsetY
        {
            get => _translateTransform3D.OffsetY;
            set => _translateTransform3D.OffsetY = value;
        }

        protected double OffsetZ
        {
            get => _translateTransform3D.OffsetZ;
            set => _translateTransform3D.OffsetZ = value;
        }

        protected Brush MaterialBrush
        {
            get => _material.Brush;
            set => _material.Brush = value;
        }

        protected MeshGeometry3D MeshGeometry3D { get; private set; }

        public GeometryModel3D Geometry { get; private set; }

        private void BuildGeometry()
        {
            MeshGeometry3D = new MeshGeometry3D
            {
                Positions = new Point3DCollection(),
                TriangleIndices = new Int32Collection()
            };

            _material = new DiffuseMaterial();

            _translateTransform3D = new TranslateTransform3D();

            Geometry = new GeometryModel3D
            {
                Material = _material,
                Geometry = MeshGeometry3D,
                Transform = _translateTransform3D
            };
        }

        protected abstract void UpdateGeometryMesh();
    }
}