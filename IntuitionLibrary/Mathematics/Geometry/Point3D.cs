namespace QBits.Intuition.Mathematics.Geometry
{
    public readonly struct Point3D : IEquatable<Point3D>
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Distance to another point.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double DistanceTo(Point3D other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            double dz = Z - other.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Converts this point to a vector from the origin.
        /// </summary>
        /// <returns></returns>
        public Vector3D ToVector() => new(X, Y, Z);

        // Operator overloads

        /// <summary>
        /// Point + Vector = Point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Point3D operator +(Point3D p, Vector3D v) =>
            new(p.X + v.X, p.Y + v.Y, p.Z + v.Z);

        /// <summary>
        /// Vector + Point = Point
        /// </summary>
        /// <param name="v"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point3D operator +(Vector3D v, Point3D p) => p + v;

        /// <summary>
        /// Point - Vector = Point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Point3D operator -(Point3D p, Vector3D v) =>
            new(p.X - v.X, p.Y - v.Y, p.Z - v.Z);

        /// <summary>
        /// Point - Point = Vector
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3D operator -(Point3D a, Point3D b) =>
            new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        // Equality
        public bool Equals(Point3D other) =>
            X == other.X && Y == other.Y && Z == other.Z;

        public override bool Equals(object? obj) =>
            obj is Point3D p && Equals(p);

        public override int GetHashCode() =>
            HashCode.Combine(X, Y, Z);

        public static bool operator ==(Point3D a, Point3D b) => a.Equals(b);
        public static bool operator !=(Point3D a, Point3D b) => !a.Equals(b);

        // Deconstruction support
        public void Deconstruct(out double x, out double y, out double z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}