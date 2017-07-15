using System.Collections.Generic;

namespace RadiantMapToWavefrontObj
{
    public class ClippingPlane
    {
        public readonly double A, B, C, D;
        public readonly Vector Normal;

        // Constructor for a clipping plane.
        public ClippingPlane(Vertex v1, Vertex v2, Vertex v3)
        {
            Vector vector1 = new Vector(v2.X - v1.X, v2.Y - v1.Y, v2.Z - v1.Z).Unit();
            Vector vector2 = new Vector(v3.X - v1.X, v3.Y - v1.Y, v3.Z - v1.Z).Unit();

            Normal = Vector.CrossProduct(vector1, vector2).Unit();
            A = Normal.X;
            B = Normal.Y;
            C = Normal.Z;                                                       // NB: A,B,C = Normal.X,Y,Z
            D = -(A * v2.X + B * v2.Y + C * v2.Z);
        }

        // Returns the object as a string.
        public override string ToString()
        {
            return "<" + A + ", " + B + ", " + C + ", " + D + ">";
        }

        // Checks if three clipping planes intersect and if so, returns an intersection point.
        public static bool FindIntersection(ClippingPlane a, ClippingPlane b, ClippingPlane c, out Vertex intersection)
        {
            // Calculates the possible intersection point using the Cramer's rule.
            double det = Determinant(a.A, a.B, a.C, b.A, b.B, b.C, c.A, c.B, c.C);
            if (det >= -1e-6 && det <= 1e-6)
            {
                intersection = null;
                return false;
            }

            double x = Determinant(a.D, a.B, a.C, b.D, b.B, b.C, c.D, c.B, c.C)/det;
            double y = Determinant(a.A, a.D, a.C, b.A, b.D, b.C, c.A, c.D, c.C)/det;
            double z = Determinant(a.A, a.B, a.D, b.A, b.B, b.D, c.A, c.B, c.D)/det;

            intersection = new Vertex(x, y, z);
            intersection.SetNormal((a.Normal + b.Normal + c.Normal).Unit());

            return true;
        }

        // Creates an array of all vertices that lie on this plane.
        public Vertex[] FindVerticesInPlane(Vertex[] vertices)
        {
            List<Vertex> res = new List<Vertex>();
            foreach (Vertex v in vertices)
            {
                if (v.OnPlane(this))
                    res.Add(v);
            }
            return res.ToArray();
        }

        // Calculates the determinant of a 2x2 matrix. (Can be done less verbose...)
        private static double Determinant(double a11, double a12, double a21, double a22)
        {
            return a11 * a22 - a12 * a21;
        }

        // Calculates the determinant of a 3x3 matrix. (Can definitely be done less verbose...)
        private static double Determinant(double a11, double a12, double a13, double a21, double a22, double a23, double a31,
            double a32, double a33)
        {
            return a11 * Determinant(a22, a23, a32, a33) - a12 * Determinant(a21, a23, a31, a33) + a13 * Determinant(a21, a22, a31, a32);
        }
    }
}
