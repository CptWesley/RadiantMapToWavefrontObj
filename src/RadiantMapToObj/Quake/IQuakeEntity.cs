using RadiantMapToObj.Configuration;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Quake
{
    /// <summary>
    /// Interface for radiant entities.
    /// </summary>
    public interface IQuakeEntity
    {
        /// <summary>
        /// Converts this entity into an <see cref="ObjObject"/> instance.
        /// </summary>
        /// <param name="settings">The conversion settings.</param>
        /// <returns>A new <see cref="ObjObject"/> instance.</returns>
        ObjObject ToObjObject(ConversionSettings settings);
    }
}
