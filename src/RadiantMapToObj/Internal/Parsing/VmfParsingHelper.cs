using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

        private static readonly IParser<string> IgnoredContent
            = String("{")
            .ThenSkip(OptionalLayout)
            .ThenSkip(Many(Or(Lazy(() => IgnoredContent), Regex("[^{}]+")), OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"));

        private static readonly IParser<string> IgnoredField
            = Regex("[a-zA-Z0-9_]+")
            .ThenSkip(OptionalLayout)
            .ThenSkip(IgnoredContent);

        private static readonly IParser<ClippingPlane> Side
            = String("side")
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("{"))
            .ThenSkip(OptionalLayout)
            .Then(Many(Field, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .Transform(x =>
            {
                string planeText = x.First(y => y.Item1 == "plane").Item2;
                string texture = x.First(y => y.Item1 == "material").Item2;
                (Vector v1, Vector v2, Vector v3) = Vertices.Parse(planeText);
                return new ClippingPlane(v1, v2, v3, texture);
            });

        private static readonly IParser<IRadiantEntity> Solid
            = String("solid")
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("{"))
            .ThenSkip(OptionalLayout)
            .Then(Many(Or(Side, Field.Then(Create<ClippingPlane>(null!)), IgnoredField.Then(Create<ClippingPlane>(null!))), OptionalLayout).Transform(x => x.Where(x => x != null)))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .Transform(x => new Brush(x));

        private static readonly IParser<IEnumerable<IRadiantEntity>> World
            = String("world")
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("{"))
            .ThenSkip(OptionalLayout)
            .Then(Many(Or(Solid, Field.Then(Create<IRadiantEntity>(null!)), IgnoredField.Then(Create<IRadiantEntity>(null!))), OptionalLayout).Transform(x => x.Where(x => x != null)))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"));

        /// <summary>
        /// Parses a .vmf file.
        /// </summary>
        internal static readonly IParser<RadiantMap> Vmf
            = OptionalLayout
            .Then(Many(Or(World, IgnoredField.Then(Create(Array.Empty<IRadiantEntity>()))), OptionalLayout))
            .ThenSkip(OptionalLayout)
            .Transform(x => new RadiantMap(x.SelectMany(x => x)))
            .ThenEnd();
    }
}
