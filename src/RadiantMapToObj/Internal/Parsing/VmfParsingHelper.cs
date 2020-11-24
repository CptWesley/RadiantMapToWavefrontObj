using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RadiantMapToObj.Radiant;
using Warpstone;
using static RadiantMapToObj.Internal.Parsing.CommonParsingHelper;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing
{
    /// <summary>
    /// Provides helper methods for parsing Hammer maps.
    /// </summary>
    [SuppressMessage("Ordering Rules", "SA1202", Justification = "Order is important for instantiation.")]
    internal static class VmfParsingHelper
    {
        private static readonly IParser<(Vector, Vector, Vector)> Vertices
            = OptionalLayout
            .Then(Vertex)
            .ThenSkip(OptionalLayout)
            .ThenAdd(Vertex)
            .ThenSkip(OptionalLayout)
            .ThenAdd(Vertex)
            .ThenSkip(OptionalLayout);

        private static readonly IParser<string> DiscardAll = CompiledRegex(@"(?s).*");
        private static readonly IParser<string> DiscardUntilSolid = new SkipUntilParser("solid");

        private static readonly IParser<ClippingPlane> Side
            = String("side")
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("{"))
            .ThenSkip(new SkipUntilParser("\"plane\""))
            .Then(CommonParsingHelper.Field)
            .ThenSkip(new SkipUntilParser("\"material\""))
            .ThenAdd(CommonParsingHelper.Field)
            .ThenSkip(new SkipUntilParser("}"))
            .ThenSkip(String("}"))
            .Transform((n, v, x) =>
            {
                string texture = x.Item2;
                string planeText = v;
                (Vector v1, Vector v2, Vector v3) = Vertices.Parse(planeText);
                Console.WriteLine($"Adding plane \"{v1} {v2} {v3}\"");
                return new ClippingPlane(v1, v2, v3, texture);
            });

        private static readonly IParser<IRadiantEntity> Solid
            = String("solid")
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("{"))

            .ThenSkip(new SkipUntilParser("side"))
            .Then(Many(Side, OptionalLayout))

            .ThenSkip(new SkipUntilParser("}"))
            .ThenSkip(String("}"))
            .Transform(x => new Brush(x));

        private static readonly IParser<IEnumerable<IRadiantEntity>> Solids
            = DiscardUntilSolid
            .Then(Many(Solid, DiscardUntilSolid))
            .ThenSkip(DiscardAll);

        /// <summary>
        /// Parses a .vmf file.
        /// </summary>
        internal static readonly IParser<RadiantMap> Vmf
            = Solids
            .Transform(x => new RadiantMap(x))
            .ThenEnd();
    }
}
