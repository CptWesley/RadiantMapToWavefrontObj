using System;
using System.Text.RegularExpressions;

namespace RadiantMapToWavefrontObj
{
    public class Vertex : Point3D
    {
        private Vector _normal;

        // Constructor of a vertex.
        public Vertex(double x, double y, double z) : base(x, y, z)
        {
            _normal = null;
        }

        // Checks if this vertex has a normal.
        public bool HasNormal()
        {
            return _normal != null;
        }

        // Returns the normal of this vertex.
        public Vector GetNormal()
        {
            return _normal;
        }

        // Sets the normal of this vertex.
        public void SetNormal(Vector normal)
        {
            _normal = normal;
        }

        // Creates a vertex from .map code.
        public static Vertex CreateFromCode(string code)
        {
            string pattern = @"(-?\d+(\.\d+)?)\s(-?\d+(\.\d+)?)\s(-?\d+(\.\d+)?)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = regex.Match(code);
            return new Vertex(Double.Parse(m.Groups[1].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                Double.Parse(m.Groups[3].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                Double.Parse(m.Groups[5].ToString(), System.Globalization.CultureInfo.InvariantCulture));
        }

        // Check if this vertex lies on a certain clipping plane.
        public bool OnPlane(Plane plane)
        {
            double left = X * plane.A + Y * plane.B + Z * plane.C;
            double right = plane.D;
            bool res = left >= right - 1e-6 && left <= right + 1e-6;
            return res;
        }

        // Override + operator.
        public static Vertex operator +(Vertex a, Vertex b)
        {
            return new Vertex(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        // Override + operator for vectors.
        public static Vertex operator +(Vertex a, Vector b)
        {
            return new Vertex(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        // Override - operator.
        public static Vertex operator -(Vertex a, Vertex b)
        {
            return new Vertex(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        // Override - operator for vectors.
        public static Vertex operator -(Vertex a, Vector b)
        {
            return new Vertex(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        // Override * operator for two points.
        public static Vertex operator *(Vertex a, Vertex b)
        {
            return new Vertex(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        // Override * operator for scalars.
        public static Vertex operator *(Vertex a, double b)
        {
            return new Vertex(a.X * b, a.Y * b, a.Z * b);
        }

        // Override * operator for scalars reversed.
        public static Vertex operator *(double a, Vertex b)
        {
            return new Vertex(a * b.X, a * b.Y, a * b.Z);
        }

        // Override / operator for two points.
        public static Vertex operator /(Vertex a, Vertex b)
        {
            return new Vertex(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        // Override * operator for scalars.
        public static Vertex operator /(Vertex a, double b)
        {
            return new Vertex(a.X / b, a.Y / b, a.Z / b);
        }
    }
}
