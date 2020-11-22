using System;
using System.Diagnostics.CodeAnalysis;

namespace RadiantMapToObj
{
    /// <summary>
    /// Represents a point in 3D space.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2225", Justification = "Would make code more convoluted.")]
    public class Vector : IEquatable<Vector>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="x">The x-axis value.</param>
        /// <param name="y">The y-axis value.</param>
        /// <param name="z">The z-axis value.</param>
        public Vector(double x, double y, double z)
            => (X, Y, Z) = (x, y, z);

        /// <summary>
        /// Gets the x-axis value.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Gets the y-axis value.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Gets the z-axis value.
        /// </summary>
        public double Z { get; }

        /// <summary>
        /// Gets the length of a vector.
        /// </summary>
        public double Length => Math.Sqrt(SquareLength);

        /// <summary>
        /// Gets the squared length of a vector.
        /// </summary>
        public double SquareLength => (X * X) + (Y * Y) + (Z * Z);

        /// <summary>
        /// Gets the unit vector of this vector.
        /// </summary>
        public Vector Unit
        {
            get
            {
                double length = Length;
                return new Vector(X / length, Y / length, Z / length);
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Vector a, Vector b)
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
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Vector a, Vector b)
            => !(a == b);

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector operator +(Vector a, Vector b)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector operator -(Vector a, Vector b)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector operator -(Vector a)
            => -1 * a;

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector operator *(Vector a, Vector b)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return new Vector(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector operator *(Vector a, double b)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return new Vector(a.X * b, a.Y * b, a.Z * b);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector operator *(double a, Vector b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return new Vector(a * b.X, a * b.Y, a * b.Z);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector operator /(Vector a, Vector b)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return new Vector(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The result of the operator.</returns>
        public static Vector operator /(Vector a, double b)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            return new Vector(a.X / b, a.Y / b, a.Z / b);
        }

        /// <summary>
        /// Calculates the cross product between two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The cross product of the two vectors.</returns>
        public static Vector CrossProduct(Vector a, Vector b)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            double x = (a.Y * b.Z) - (a.Z * b.Y);
            double y = (a.Z * b.X) - (a.X * b.Z);
            double z = (a.X * b.Y) - (a.Y * b.X);
            return new Vector(x, y, z);
        }

        /// <summary>
        /// Calculates the dot product between two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static double DotProduct(Vector a, Vector b)
        {
            if (a is null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        /// <summary>
        /// Get the distance between this point and another given point.
        /// </summary>
        /// <param name="other">The other point.</param>
        /// <returns>The distance between the two points.</returns>
        public double Distance(Vector other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            double dX = X - other.X;
            double dY = Y - other.Y;
            double dZ = Z - other.Z;
            return Math.Sqrt((dX * dX) + (dY * dY) + (dZ * dZ));
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"<{X}, {Y}, {Z}>";

        /// <inheritdoc/>
        public bool Equals(Vector? other)
        {
            if (other is null)
            {
                return false;
            }

            return ApproximatelyEquals(X, other.X) && ApproximatelyEquals(Y, other.Y) && ApproximatelyEquals(Z, other.Z);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is Vector other)
            {
                return Equals(other);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => (int)Math.Floor((X * 2) + (Y * 4) + (Z * 8));

        /// <summary>
        /// Checks if the vector coordinates lie on a plane.
        /// </summary>
        /// <param name="plane">The plane.</param>
        /// <returns>True if the vector is on the plane, false otherwise.</returns>
        public bool OnPlane(Plane plane)
        {
            if (plane is null)
            {
                throw new ArgumentNullException(nameof(plane));
            }

            double left = (X * plane.A) + (Y * plane.B) + (Z * plane.C);
            double right = plane.D;
            bool res = left >= right - 1e-6 && left <= right + 1e-6;
            return res;
        }

        /// <summary>
        /// Checks if two vectors have the same direction.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>True if direction is equal, false otherwise.</returns>
        public bool DirectionEquals(Vector other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return Unit.Equals(other.Unit);
        }

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
