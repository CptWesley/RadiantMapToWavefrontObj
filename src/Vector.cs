using System;

namespace RadiantMapToWavefrontObj
{
    public class Vector : Point3D
    {
        public Vector(double x, double y, double z) : base(x, y, z){}

        // Implicit cast from Vertex to Vector.
        public static explicit operator Vector(Vertex a)
        {
            return new Vector(a.X, a.Y, a.Z);
        }

        // Calculates the cross product between two vectors.
        public static Vector CrossProduct(Vector a, Vector b)
        {
            double x = a.Y*b.Z - a.Z*b.Y;
            double y = a.Z*b.X - a.X*b.Z;
            double z = a.X*b.Y - a.Y*b.X;
            return new Vector(x, y, z);
        }

        // Calculates the dot product between two vectors.
        public static double DotProduct(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        // Calculates the length of a vector.
        public double Length()
        {
            return Math.Sqrt(X*X+Y*Y+Z*Z);
        }

        // Calculates the squared length of a vector.
        public double SquareLength()
        {
            return X*X + Y*Y + Z*Z;
        }

        // Returns a unit version of this vector.
        public Vector Unit()
        {
            double length = Length();
            return new Vector(X/length, Y/length, Z/length);
        }

        // Checks if two vectors have the same direction.
        public bool DirectionEquals(Vector other)
        {
            if (Unit().Equals(other.Unit()))
                return true;
            return false;
        }

        // Override + operator.
        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        // Override - operator.
        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        // Override * operator for two points.
        public static Vector operator *(Vector a, Vector b)
        {
            return new Vector(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        // Override * operator for scalars.
        public static Vector operator *(Vector a, double b)
        {
            return new Vector(a.X * b, a.Y * b, a.Z * b);
        }

        // Override * operator for scalars reversed.
        public static Vector operator *(double a, Vector b)
        {
            return new Vector(a * b.X, a * b.Y, a * b.Z);
        }

        // Override / operator for two points.
        public static Vector operator /(Vector a, Vector b)
        {
            return new Vector(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        // Override * operator for scalars.
        public static Vector operator /(Vector a, double b)
        {
            return new Vector(a.X / b, a.Y / b, a.Z / b);
        }
    }
}
