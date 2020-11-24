using RadiantMapToObj.Internal.Parsing.Hammer;
using RadiantMapToObj.Internal.Parsing.Radiant;
using RadiantMapToObj.Radiant;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing
{
    /// <summary>
    /// Provides functionality for parsing any kind of map.
    /// </summary>
    internal static class MapParser
    {
        /// <summary>
        /// Parses a .map file to our radiant map object.
        /// </summary>
        /// <param name="input">The content of the .map file.</param>
        /// <returns>The parsed radiant map.</returns>
        public static RadiantMap Parse(string input)
            => Or(VmfParsingHelper.Vmf, RadiantMapParsingHelper.Map).Parse(input);
    }
}
