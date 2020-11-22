using System;
using System.Collections.Generic;

namespace RadiantMapToObj
{
    /// <summary>
    /// Represents a plane in 3D space.
    /// </summary>
    public class Plane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plane"/> class.
        /// </summary>
        /// <param name="v1">The first vertex.</param>
        /// <param name="v2">The second vertex.</param>
        /// <param name="v3">The third vertex.</param>
        public Plane(Vector v1, Vector v2, Vector v3)
        {
            Vector vector1 = new Vector(v2.X - v1.X, v2.Y - v1.Y, v2.Z - v1.Z).Unit;
            Vector vector2 = new Vector(v3.X - v1.X, v3.Y - v1.Y, v3.Z - v1.Z).Unit;

            Normal = Vector.CrossProduct(vector1, vector2).Unit;
            D = -((A * v2.X) + (B * v2.Y) + (C * v2.Z));
        }

        /// <summary>
        /// Gets the A value of the plane.
        /// </summary>
        public double A => Normal.X;

        /// <summary>
        /// Gets the B value of the plane.
        /// </summary>
        public double B => Normal.Y;

        /// <summary>
        /// Gets the C value of the plane.
        /// </summary>
        public double C => Normal.Z;

        /// <summary>
        /// Gets the plane facing direction.
        /// </summary>
        public Vector Normal { get; }

        /// <summary>
        /// Gets the D value of the plane.
        /// </summary>
        public double D { get; }

        /// <inheritdoc/>
        public override string ToString()
            => $"<{A}, {B}, {C}, {D}>";

        /// <summary>
        /// Creates a collection of all vertices that lie on this plane.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <returns>A collection of all vertices in the plane.</returns>
        public IEnumerable<Vector> FindVerticesInPlane(IEnumerable<Vector> vertices)
        {
            if (vertices is null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            List<Vector> res = new List<Vector>();
            foreach (Vector v in vertices)
            {
                if (v.OnPlane(this))
                {
                    res.Add(v);
                }
            }

            return res;
        }
    }
}
