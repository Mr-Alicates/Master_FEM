namespace POC3D.Model
{
    public class ModelPoint
    {
        public ModelPoint()
            : this(0, 0, 0)
        {
        }

        public ModelPoint(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ModelPoint modelPoint && Equals(modelPoint);
        }

        protected bool Equals(ModelPoint other)
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