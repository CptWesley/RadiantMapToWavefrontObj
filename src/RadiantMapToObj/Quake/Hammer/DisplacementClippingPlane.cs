namespace RadiantMapToObj.Quake.Hammer
{
    /// <summary>
    /// Represents a clipping plane which should become a displacement.
    /// </summary>
    /// <seealso cref="ClippingPlane" />
    public class DisplacementClippingPlane : ClippingPlane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplacementClippingPlane" /> class.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <param name="v3">The third vector.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="displacement">The displacement info.</param>
        public DisplacementClippingPlane(Vector v1, Vector v2, Vector v3, string texture, DisplacementInfo displacement)
                    : base(v1, v2, v3, texture)
                    => DisplacementInfo = displacement;

        /// <summary>
        /// Gets the displacement information.
        /// </summary>
        public DisplacementInfo DisplacementInfo { get; }
    }
}
