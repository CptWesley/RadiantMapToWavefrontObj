using System.Collections.Generic;
using RadiantMapToObj.Internal.Conversion;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Quake
{
    /// <summary>
    /// Class for Brush.
    /// </summary>
    public class Brush : IQuakeEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Brush"/> class.
        /// </summary>
        /// <param name="clippingPlanes">The clipping planes.</param>
        public Brush(IEnumerable<ClippingPlane> clippingPlanes)
            => ClippingPlanes = clippingPlanes;

        /// <summary>
        /// Gets the clipping planes.
        /// </summary>
        public IEnumerable<ClippingPlane> ClippingPlanes { get; }

        /// <inheritdoc/>
        public ObjObject ToObjObject()
            => BrushConversionHelper.Convert(this);
    }
}
