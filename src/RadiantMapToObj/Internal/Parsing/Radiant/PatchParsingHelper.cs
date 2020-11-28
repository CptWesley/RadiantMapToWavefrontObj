using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RadiantMapToObj.Quake.Radiant;
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
            .Then(Multiple(Double, Layout, 5))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform(x => -new Vector(x[0], x[1], x[2]));

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

        private static readonly IParser<IList<double>> GridSizePatchDef2
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(Multiple(Double, Layout, 5))
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
            .Then(Multiple(Double, Layout, 10))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String(")"))
            .Transform(x => -new Vector(x[0], x[1], x[2]));

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

        private static readonly IParser<IList<double>> GridSizePatchDef3
            = String("(")
            .ThenSkip(OptionalLayout)
            .Then(Multiple(Double, Layout, 7))
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
