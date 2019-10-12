using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace POC3D.ViewModel
{
    public class ElementViewModel
    {
        private Point3D _center;
        private double _size;

        public ElementViewModel()
            : this(new Point3D(), 1)
        {

        }

        public ElementViewModel(Point3D center, double size)
        {
            _center = center;
            _size = size;
        }

        public Point3D Center => _center;

        public double Size => _size;
        public double HalfSize => _size / 2.0;
    }
}
