using System;

namespace RadiantMapToWavefrontObj
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
        public Edge(Vertex a, Vertex b)
        {
            A = a;
            B = b;
        }

        /// <summary>
        /// Gets one of the vertices of the edge.
        /// </summary>
        public Vertex A { get; }

        /// <summary>
        /// Gets one of the vertices of the edge.
        /// </summary>
        public Vertex B { get; }

        /// <summary>
        /// Gets the vector.
        /// </summary>
        /// <returns>the Vector.</returns>
        public Vector Vector
            => (Vector)B - (Vector)A;

        /// <summary>
        /// Gets the length.
        /// </summary>
        public double Length
            => Vector.Length();

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
        {
            return !(a == b);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
            => $"<{A}, {B}>";

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Edge that)
            {
                Equals(that);
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return A.GetHashCode() + (2 * B.GetHashCode());
        }

        /// <inheritdoc/>
        public bool Equals(Edge other)
            => A == other.A && B == other.B;

    }
}