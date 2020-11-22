using System;

namespace RadiantMapToObj
{
    /// <summary>
    /// Class for Edge.
    /// </summary>
    public struct Edge : IEquatable<Edge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> struct.
        /// </summary>
        /// <param name="a">Vertex a of the edge.</param>
        /// <param name="b">Vertex b of the edge.</param>
        public Edge(Vector a, Vector b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// Gets one of the vertices of the edge.
        /// </summary>
        public Vector A { get; }

        /// <summary>
        /// Gets one of the vertices of the edge.
        /// </summary>
        public Vector B { get; }

        /// <summary>
        /// Gets the vector.
        /// </summary>
        /// <returns>the Vector.</returns>
        public Vector Vector
            => B - A;

        /// <summary>
        /// Gets the length.
        /// </summary>
        public double Length
            => Vector.Length;

        /// <summary>
        /// Gets the inverse.
        /// </summary>
        /// <returns>Inverse of Edge.</returns>
        public Edge Inverse
            => new Edge(B, A);

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">One of the edges.</param>
        /// <param name="b">Another edge.</param>
        /// <returns>
        /// The result of the equals operator.
        /// </returns>
        public static bool operator ==(Edge a, Edge b)
            => a.Equals(b);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">One of the edges.</param>
        /// <param name="b">The other of the edges.</param>
        /// <returns>
        /// The result of the non-equality operator.
        /// </returns>
        public static bool operator !=(Edge a, Edge b)
            => !(a == b);

        /// <inheritdoc/>
        public override string ToString()
            => $"<{A}, {B}>";

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Edge that)
            {
                Equals(that);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => A.GetHashCode() + (2 * B.GetHashCode());

        /// <inheritdoc/>
        public bool Equals(Edge other)
            => A == other.A && B == other.B;
    }
}