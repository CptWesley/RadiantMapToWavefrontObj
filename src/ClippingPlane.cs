
namespace RadiantMapToWavefrontObj
{
    public class ClippingPlane : Plane
    {
        public string Texture { get; private set; }

        // Constructor for a clipping plane.
        public ClippingPlane(Vertex v1, Vertex v2, Vertex v3, string texture) : base(v1, v2, v3)
        {
            Texture = texture;
        }

        // Set the texture of a clipping plane.
        public void SetTexture(string texture)
        {
            Texture = texture;
        }

        // Checks if three clipping planes intersect and if so, returns an intersection point.
        public static bool FindIntersection(Plane a, Plane b, Plane c, out Vertex intersection)
        {
            // Calculates the possible intersection point using the Cramer's rule.
            double det = CallConvThiscall.Determinant(a.A, a.B, a.C, b.A, b.B, b.C, c.A, c.B, c.C);
            if (det >= -1e-6 && det <= 1e-6)
            {
                intersection = null;
                return false;
            }

            double x = Determinant(a.D, a.B, a.C, b.D, b.B, b.C, c.D, c.B, c.C) / det;
            double y = Determinant(a.A, a.D, a.C, b.A, b.D, b.C, c.A, c.D, c.C) / det;
            double z = Determinant(a.A, a.B, a.D, b.A, b.B, b.D, c.A, c.B, c.D) / det;

            intersection = new Vertex(x, y, z);
            intersection.SetNormal((a.Normal + b.Normal + c.Normal).Unit());

            return true;
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
