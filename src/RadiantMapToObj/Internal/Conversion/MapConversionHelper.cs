using System.Linq;
using RadiantMapToObj.Radiant;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Internal.Conversion
{
    /// <summary>
    /// Provides helper functions for converting maps to objs.
    /// </summary>
    internal static class MapConversionHelper
    {
        /// <summary>
        /// Converts a RadiantMap object to a WavefrontObj object.
        /// </summary>
        /// <param name="map">The radiant map to convert.</param>
        /// <returns>A wavefront object created from a given radiant map.</returns>
        internal static WavefrontObj Convert(RadiantMap map)
            => new WavefrontObj(map.Entities.Select(x => x.ToObjObject()));
    }
}
