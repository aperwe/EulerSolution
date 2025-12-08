namespace QBits.Intuition.Mathematics.Geometry
{
    public readonly struct Vector3D : IEquatable<Vector3D>
    {
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <summary>
        /// Length (magnitude) of the vector.
        /// </summary>
        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Normalized (unit) vector.
        /// </summary>
        /// <returns></returns>
        public Vector3D Normalized()
        {
            double len = Length;
            return len > 0 ? new Vector3D(X / len, Y / len, Z / len) : this;
        }

        /// <summary>
        /// Dot product of two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Dot(Vector3D a, Vector3D b) =>
            a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        /// <summary>
        /// Cross product of two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3D Cross(Vector3D a, Vector3D b) =>
            new Vector3D(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );

        // Operator overloads
        public static Vector3D operator +(Vector3D a, Vector3D b) =>
            new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector3D operator -(Vector3D a, Vector3D b) =>
            new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector3D operator -(Vector3D v) =>
            new(-v.X, -v.Y, -v.Z);

        public static Vector3D operator *(Vector3D v, double scalar) =>
            new(v.X * scalar, v.Y * scalar, v.Z * scalar);

        public static Vector3D operator *(double scalar, Vector3D v) =>
            v * scalar;

        public static Vector3D operator /(Vector3D v, double scalar) =>
            new(v.X / scalar, v.Y / scalar, v.Z / scalar);

        public static bool operator ==(Vector3D a, Vector3D b) =>
            a.Equals(b);

        public static bool operator !=(Vector3D a, Vector3D b) =>
            !a.Equals(b);

        // Equality implementation
        public bool Equals(Vector3D other) =>
            X == other.X && Y == other.Y && Z == other.Z;

        public override bool Equals(object? obj) =>
            obj is Vector3D v && Equals(v);

        public override int GetHashCode() =>
            HashCode.Combine(X, Y, Z);

        // Deconstruction (C# tuples)
        public void Deconstruct(out double x, out double y, out double z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}