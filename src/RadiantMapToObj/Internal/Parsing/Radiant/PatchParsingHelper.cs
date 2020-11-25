using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RadiantMapToObj.Radiant;
using Warpstone;
using Warpstone.Parsers;
using static RadiantMapToObj.Internal.Parsing.CommonParsingHelper;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing.Radiant
{
    /// <summary>
    /// Provides helper methods for parsing patches.
    /// </summary>
    [SuppressMessage("Ordering Rules", "SA1202", Justification = "Order is important for instantiation.")]
    internal static class PatchParsingHelper
    {
        private static readonly IParser<Vector> VertexUvPatchDef2
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

        private static readonly IParser<Vector[]> VertexRowPatchDef2
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(Many(VertexUvPatchDef2, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform(x => x.ToArray());

        private static readonly IParser<Vector[][]> VertexGridPatchDef2
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(Many(VertexRowPatchDef2, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform(x => x.ToArray());

        private static readonly IParser<(double, double, double, double, double)> GridSizePatchDef2
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
            .ThenSkip(GridSizePatchDef2)
            .ThenSkip(OptionalLayout)
            .ThenAdd(VertexGridPatchDef2)
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .Transform((t, g) => new Patch(g));

        private static readonly IParser<Vector> VertexUvPatchDef3
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
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
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
            .Transform((a, q4, q5) => -new Vector(a.Item1, a.Item2, a.Item3));

        private static readonly IParser<Vector[]> VertexRowPatchDef3
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(Many(VertexUvPatchDef3, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform(x => x.ToArray());

        private static readonly IParser<Vector[][]> VertexGridPatchDef3
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(Many(VertexRowPatchDef3, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform(x => x.ToArray());

        private static readonly IParser<(double, double, double, double, double, double, double)> GridSizePatchDef3
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
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(Layout)
            .ThenAdd(CommonParsingHelper.Double)
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"));

        private static readonly IParser<Patch> PatchDef3
            = String("{")
            .ThenSkip(OptionalLayout)
            .ThenSkip(Or(String("patchTerrainDef3"), String("patchDef5")))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("{"))
            .ThenSkip(OptionalLayout)
            .Then(Texture)
            .ThenSkip(OptionalLayout)
            .ThenSkip(GridSizePatchDef3)
            .ThenSkip(OptionalLayout)
            .ThenAdd(VertexGridPatchDef3)
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .Transform((t, g) => new Patch(g));

        /// <summary>
        /// Parses a patch.
        /// </summary>
        internal static readonly IParser<Patch> Patch
            = Or(PatchDef3, PatchDef2);
    }
}
