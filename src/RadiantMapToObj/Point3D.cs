using System;

namespace RadiantMapToWavefrontObj
{
    public class Point3D
    {
        // Coordinates
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        // Constructor for a point.
        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // Get the distance between two points.
        public double Distance(Vertex other)
        {
            double dX = X - other.X;
            double dY = Y - other.Y;
            double dZ = Z - other.Z;
            return Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
        }

        // Returns a string object containing contents of this point.
        public override string ToString()
        {
            return "<" + X + ", " + Y + ", " + Z + ">";
        }

        // Checks if two objects are equal.
        public override bool Equals(object obj)
        {
            if (obj is Point3D)
            {
                Point3D that = (Point3D)obj;
                if (ApproximatelyEquals(X, that.X) && ApproximatelyEquals(Y, that.Y) && ApproximatelyEquals(Z, that.Z))
                    return true;
            }
            return false;
        }

        // Checks if two doubles are roughly equal.
        private static bool ApproximatelyEquals(double a, double b)
        {
            double delta = a - b;
            if (delta >= -1e-6 && delta <= 1e-6)
                return true;
            return false;
        }

        // Returns a hascode for the object.
        public override int GetHashCode()
        {
            return (int)Math.Floor(X * 2 + Y * 4 + Z * 8);
        }

        // Override == operator.
        public static bool operator ==(Point3D a, Point3D b)
        {
            if (ReferenceEquals(a, null))
            {
                if (ReferenceEquals(b, null))
                    return true;
                return false;
            }
            return a.Equals(b);
        }

        // Override != operator.
        public static bool operator !=(Point3D a, Point3D b)
        {
            return !(a == b);
        }

        // Override + operator.
        public static Point3D operator +(Point3D a, Point3D b)
        {
            return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        // Override - operator.
        public static Point3D operator -(Point3D a, Point3D b)
        {
            return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        // Override * operator for two points.
        public static Point3D operator *(Point3D a, Point3D b)
        {
            return new Point3D(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        // Override * operator for scalars.
        public static Point3D operator *(Point3D a, double b)
        {
            return new Point3D(a.X * b, a.Y * b, a.Z * b);
        }

        // Override * operator for scalars reversed.
        public static Point3D operator *(double a, Point3D b)
        {
            return new Point3D(a * b.X, a * b.Y, a * b.Z);
        }

        // Override / operator for two points.
        public static Point3D operator /(Point3D a, Point3D b)
        {
            return new Point3D(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        // Override * operator for scalars.
        public static Point3D operator /(Point3D a, double b)
        {
            return new Point3D(a.X / b, a.Y / b, a.Z / b);
        }
    }
}
