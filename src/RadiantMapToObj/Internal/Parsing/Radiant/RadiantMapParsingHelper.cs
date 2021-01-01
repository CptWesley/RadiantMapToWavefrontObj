using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RadiantMapToObj.Quake;
using Warpstone;
using static RadiantMapToObj.Internal.Parsing.CommonParsingHelper;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing.Radiant
{
    /// <summary>
    /// Provides helper methods for parsing maps.
    /// </summary>
    [SuppressMessage("Ordering Rules", "SA1202", Justification = "Order is important for instantiation.")]
    internal static class RadiantMapParsingHelper
    {
        private static readonly IParser<IQuakeEntity> Entity
            = Or<IQuakeEntity>(PatchParsingHelper.Patch, BrushParsingHelper.Brush);

        private static readonly IParser<IEnumerable<IQuakeEntity>> EntityContent
            = String("{")
            .ThenSkip(OptionalLayout)
            .Then(Many(Field, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .Then(Many(Entity, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"));

        private static readonly IParser<IEnumerable<IQuakeEntity>> Entities = Many(EntityContent, OptionalLayout).Transform(x => x.SelectMany(x => x));

        /// <summary>
        /// Parses a radiant map.
        /// </summary>
        internal static readonly IParser<QuakeMap> Map = OptionalLayout.Then(Entities).ThenSkip(OptionalLayout).ThenEnd().Transform(x => new QuakeMap(x));
    }
}
