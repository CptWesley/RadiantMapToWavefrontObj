using System.Collections.Generic;
using RadiantMapToObj.Radiant;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Internal
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
        {
            List<ObjObject> objects = new List<ObjObject>();

            int i = 0;
            foreach (IRadiantEntity entity in map.Entities)
            {
                objects.Add(entity.ToObjObject($"Entity_{i++}"));
            }

            return new WavefrontObj(objects);
        }
    }
}
