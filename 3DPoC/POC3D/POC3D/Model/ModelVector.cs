using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC3D.Model
{
    public class ModelVector
    {
        public ModelVector()
        {
        }

        public ModelVector(ModelPoint end, ModelPoint beggining)
            : this(end.X - beggining.X, end.Y - beggining.Y, end.Z - beggining.Z)
        {

        }

        public ModelVector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Modulus => Math.Sqrt(X * X + Y * Y + Z * Z);

        public override bool Equals(object obj)
        {
            return obj is ModelPoint modelPoint && Equals(modelPoint);
        }

        protected bool Equals(ModelVector other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }
}
