using System;
using System.Collections.Generic;

namespace RadiantMapToObj.Wavefront
{
    /// <summary>
    /// Class for Faces.
    /// </summary>
    public class Face : IEquatable<Face>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Face"/> class.
        /// </summary>
        /// <param name="a">The first vertex.</param>
        /// <param name="b">The second vertex.</param>
        /// <param name="c">The third vertex.</param>
        /// <param name="texture">The texture.</param>
        public Face(Vector a, Vector b, Vector c, string texture)
            => (A, B, C, Texture) = (a, b, c, texture);

        /// <summary>
        /// Gets the texture.
        /// </summary>
        public string Texture { get; }

        /// <summary>
        /// Gets the first vertex.
        /// </summary>
        public Vector A { get; }

        /// <summary>
        /// Gets the second vertex.
        /// </summary>
        public Vector B { get; }

        /// <summary>
        /// Gets the third vertex.
        /// </summary>
        public Vector C { get; }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        public IEnumerable<Vector> Vertices
            => new Vector[] { A, B, C };

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the double equals operator.
        /// </returns>
        public static bool operator ==(Face a, Face b)
        {
            if (a is null)
            {
                return b is null;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Face a, Face b)
            => !(a == b);

        /// <summary>
        /// Gets the circumsphere of this triangle.
        /// </summary>
        /// <returns>Circumsphere of the triangle.</returns>
        public Tuple<Vector, double> GetCircumsphere()
        {
            Vector v0 = B - A;
            Vector v1 = C - A;

            Vector vx = Vector.CrossProduct(v0, v1);

            Vector centerVector = ((Vector.CrossProduct(vx, v0) * v1.SquareLength) + (Vector.CrossProduct(v1, vx) * v0.SquareLength)) / (2 * vx.SquareLength);
            Vector center = A + centerVector;

            double radius = centerVector.Length;

            return new Tuple<Vector, double>(center, radius);
        }

        /// <summary>
        /// Gets the edges.
        /// </summary>
        /// <returns> Edges of the face.</returns>
        public Edge[] GetEdges()
        {
            Edge[] edges = new Edge[3];
            edges[0] = new Edge(A, B);
            edges[1] = new Edge(B, C);
            edges[2] = new Edge(C, A);
            return edges;
        }

        /// <summary>
        /// Determines whether this face contains a vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified vertex]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Vector vertex)
            => A == vertex || B == vertex || C == vertex;

        /// <inheritdoc/>
        public override string ToString()
            => $"({A}, {B}, {C})";

        /// <inheritdoc/>
        public bool Equals(Face other)
        {
            if (other is null)
            {
                return false;
            }

            return A == other.A && B == other.B && C == other.C;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Face other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => (A.GetHashCode() * 2) + (B.GetHashCode() * 4) + (C.GetHashCode() * 8);
    }
}
