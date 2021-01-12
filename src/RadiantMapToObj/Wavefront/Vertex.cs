namespace RadiantMapToObj.Wavefront
{
    /// <summary>
    /// A 3d vertex with a texture.
    /// </summary>
    /// <seealso cref="Vector" />
    public class Vertex : Vector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="u">The u coordinate.</param>
        /// <param name="v">The v coordinate.</param>
        public Vertex(double x, double y, double z, double u, double v)
            : base(x, y, z)
            => (U, V) = (u, v);

        /// <summary>
        /// Gets the u coordinate.
        /// </summary>
        public double U { get; }

        /// <summary>
        /// Gets the v coordinate.
        /// </summary>
        public double V { get; }
    }
}
