using System;
using System.Collections.Generic;
using System.Linq;
using RadiantMapToObj.Radiant;
using Warpstone;
using static RadiantMapToObj.Internal.Parsing.CommonParsingHelper;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing
{
    /// <summary>
    /// Provides helper methods for parsing maps.
    /// </summary>
    internal static class MapParsingHelper
    {
        private static readonly IParser<IRadiantEntity> Entity
            = Or<IRadiantEntity>(PatchParsingHelper.Patch, BrushParsingHelper.Brush);

        private static readonly IParser<IEnumerable<IRadiantEntity>> EntityContent
            = String("{")
            .ThenSkip(OptionalLayout)
            .Then(Many(Field, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .Then(Many(Entity, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"));

        private static readonly IParser<IEnumerable<IRadiantEntity>> Entities = Many(EntityContent, OptionalLayout).Transform(x => x.SelectMany(x => x));

        private static readonly IParser<RadiantMap> Map = OptionalLayout.Then(Entities).ThenSkip(OptionalLayout).ThenEnd().Transform(x => new RadiantMap(x));

        /// <summary>
        /// Parses a .map file to our radiant map object.
        /// </summary>
        /// <param name="input">The content of the .map file.</param>
        /// <returns>The parsed radiant map.</returns>
        public static RadiantMap Parse(string input)
        {
            RadiantMap map = Or(VmfParsingHelper.Vmf, Map).Parse(input);

            Console.WriteLine($"Finished: {map}");

            return map;
        }
    }
}
