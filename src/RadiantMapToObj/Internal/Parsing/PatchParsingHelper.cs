using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RadiantMapToObj.Radiant;
using Warpstone;
using Warpstone.Parsers;
using static RadiantMapToObj.Internal.Parsing.CommonParsingHelper;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing
{
    /// <summary>
    /// Provides helper methods for parsing patches.
    /// </summary>
    [SuppressMessage("Ordering Rules", "SA1202", Justification = "Order is important for instantiation.")]
    internal static class PatchParsingHelper
    {
        private static readonly IParser<Vector> VertexUv
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(CommonParsingHelper.Double)
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform((x, y, z, u, v) => -new Vector(x, y, z));

        private static readonly IParser<Vector[]> VertexRow
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(Many(VertexUv, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform(x => x.ToArray());

        private static readonly IParser<Vector[][]> VertexGrid
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(Many(VertexRow, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform(x => x.ToArray());

        private static readonly IParser<(double, double, double, double, double)> GridSize
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(CommonParsingHelper.Double)
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"));

        private static readonly IParser<Patch> PatchDef2
            = String("{")
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("patchDef2"))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("{"))
            .ThenSkip(OptionalLayout)
            .Then(Texture)
            .ThenSkip(OptionalLayout)
            .ThenSkip(GridSize)
            .ThenSkip(OptionalLayout)
            .ThenAdd(VertexGrid)
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .Transform((t, g) => new Patch(g));

        /// <summary>
        /// Parses a patch.
        /// </summary>
        internal static readonly IParser<Patch> Patch
            = PatchDef2;
    }
}
