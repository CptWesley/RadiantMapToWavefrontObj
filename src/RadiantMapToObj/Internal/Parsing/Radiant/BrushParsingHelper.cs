using System.Diagnostics.CodeAnalysis;
using RadiantMapToObj.Quake;
using Warpstone;
using static RadiantMapToObj.Internal.Parsing.CommonParsingHelper;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing.Radiant
{
    /// <summary>
    /// Provides helper methods for parsing brushes.
    /// </summary>
    [SuppressMessage("Ordering Rules", "SA1202", Justification = "Order is important for instantiation.")]
    internal static class BrushParsingHelper
    {
        private static readonly IParser<ClippingPlane> ClippingPlane
            = Vertex
            .ThenSkip(OptionalLayout)
            .ThenAdd(Vertex)
            .ThenSkip(OptionalLayout)
            .ThenAdd(Vertex)
            .ThenSkip(OptionalLayout)
            .ThenAdd(Texture)
            .ThenSkip(Layout)
            .ThenSkip(CompiledRegex(@".*"))
            .Transform((a, b, c, t) => new ClippingPlane(a, b, c, t));

        /// <summary>
        /// Parses a brush.
        /// </summary>
        internal static readonly IParser<Brush> Brush
            = String("{")
            .ThenSkip(OptionalLayout)
            .Then(Many(ClippingPlane, Layout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .Transform(x => new Brush(x));
    }
}
