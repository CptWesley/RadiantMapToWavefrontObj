using System;

namespace RadiantMapToObj
{
    /// <summary>
    /// Represents a uv coordinate.
    /// </summary>
    public class TextureCoordinate : IEquatable<TextureCoordinate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureCoordinate"/> class.
        /// </summary>
        /// <param name="u">The u coordinate.</param>
        /// <param name="v">The v coordinate.</param>
        public TextureCoordinate(double u, double v)
            => (U, V) = (u, v);

        /// <summary>
        /// Gets the u coordinate.
        /// </summary>
        public double U { get; }

        /// <summary>
        /// Gets the v coordinate.
        /// </summary>
        public double V { get; }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">The first coordinate.</param>
        /// <param name="b">The second coordinate.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TextureCoordinate a, TextureCoordinate b)
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
        /// <param name="a">The first coordinate.</param>
        /// <param name="b">The second coordinate.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TextureCoordinate a, TextureCoordinate b)
            => !(a == b);

        /// <inheritdoc/>
        public override string ToString()
            => $"<{U}, {V}>";

        /// <inheritdoc/>
        public bool Equals(TextureCoordinate? other)
        {
            if (other is null)
            {
                return false;
            }

            return ApproximatelyEquals(U, other.U) && ApproximatelyEquals(V, other.V);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is TextureCoordinate other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => (int)Math.Floor((U * 3) + (V * 6));

        /// <summary>
        /// Checks if two doubles are roughly equal.
        /// </summary>
        /// <param name="a">The first double.</param>
        /// <param name="b">The second double.</param>
        /// <returns>True if they are roughly equal.</returns>
        private static bool ApproximatelyEquals(double a, double b)
        {
            double delta = a - b;
            if (delta >= -1e-6 && delta <= 1e-6)
            {
                return true;
            }

            return false;
        }
    }
}
