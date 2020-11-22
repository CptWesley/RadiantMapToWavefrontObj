using System;

namespace RadiantMapToObj.Radiant
{
    /// <summary>
    /// Class for ClippingPlane.
    /// </summary>
    /// <seealso cref="Plane" />
    public class ClippingPlane : Plane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClippingPlane"/> class.
        /// </summary>
        /// <param name="v1">Vertex 1.</param>
        /// <param name="v2">Vertex 2.</param>
        /// <param name="v3">Vertex 3.</param>
        /// <param name="texture">The texture.</param>
        public ClippingPlane(Vector v1, Vector v2, Vector v3, string texture)
            : base(v1, v2, v3)
            => Texture = texture;

        /// <summary>
        /// Gets the texture.
        /// </summary>
        public string Texture { get; }

        /// <summary>
        /// Checks if three clipping planes intersect and if so, returns an intersection point.
        /// </summary>
        /// <param name="a">Plane a.</param>
        /// <param name="b">Plane b.</param>
        /// <param name="c">Plane c.</param>
        /// <param name="intersection">The intersection.</param>
        /// <returns>If an intersection exists.</returns>
        public static bool FindIntersection(Plane a, Plane b, Plane c, out Vector? intersection)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (c is null)
            {
                throw new ArgumentNullException(nameof(c));
            }

            // Calculates the possible intersection point using the Cramer's rule.
            double det = Determinant(a.A, a.B, a.C, b.A, b.B, b.C, c.A, c.B, c.C);
            if (det >= -1e-6 && det <= 1e-6)
            {
                intersection = null;
                return false;
            }

            double x = Determinant(a.D, a.B, a.C, b.D, b.B, b.C, c.D, c.B, c.C) / det;
            double y = Determinant(a.A, a.D, a.C, b.A, b.D, b.C, c.A, c.D, c.C) / det;
            double z = Determinant(a.A, a.B, a.D, b.A, b.B, b.D, c.A, c.B, c.D) / det;

            intersection = new Vector(x, y, z);

            return true;
        }

        // Calculates the determinant of a 2x2 matrix. (Can be done less verbose...)
        private static double Determinant(double a11, double a12, double a21, double a22)
            => (a11 * a22) - (a12 * a21);

        // Calculates the determinant of a 3x3 matrix. (Can definitely be done less verbose...)
        private static double Determinant(double a11, double a12, double a13, double a21, double a22, double a23, double a31, double a32, double a33)
            => (a11 * Determinant(a22, a23, a32, a33)) - (a12 * Determinant(a21, a23, a31, a33)) + (a13 * Determinant(a21, a22, a31, a32));
    }
}
