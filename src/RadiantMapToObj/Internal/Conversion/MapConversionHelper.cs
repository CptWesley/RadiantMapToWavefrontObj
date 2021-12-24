using System.Linq;
using RadiantMapToObj.Configuration;
using RadiantMapToObj.Quake;
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
        /// <param name="settings">The conversion settings.</param>
        /// <returns>A wavefront object created from a given radiant map.</returns>
        internal static WavefrontObj Convert(QuakeMap map, ConversionSettings settings)
        {
            WavefrontObj result = new WavefrontObj(map.Entities.Select(x => x.ToObjObject(settings)));
            result.FilterTextures(settings.Filter);
            return result;
        }
    }
}
