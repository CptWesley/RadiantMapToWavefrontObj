using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RadiantMapToObj.Radiant;
using Warpstone;
using static RadiantMapToObj.Internal.Parsing.CommonParsingHelper;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing.Hammer
{
    /// <summary>
    /// Provides helper methods for parsing Hammer maps.
    /// </summary>
    [SuppressMessage("Ordering Rules", "SA1202", Justification = "Order is important for instantiation.")]
    internal static class VmfParsingHelper
    {
        private abstract record VmfElement;

        [SuppressMessage("Spacing Rules", "SA1009", Justification = "Contradictory rules.")]
        private record VmfField(string name, string value) : VmfElement;

        [SuppressMessage("Spacing Rules", "SA1009", Justification = "Contradictory rules.")]
        private record VmfClass(string name, IEnumerable<VmfField> fields, IEnumerable<VmfClass> classes) : VmfElement;

        private static readonly IParser<(Vector, Vector, Vector)> Vertices
            = OptionalLayout
            .Then(Vertex)
            .ThenSkip(OptionalLayout)
            .ThenAdd(Vertex)
            .ThenSkip(OptionalLayout)
            .ThenAdd(Vertex)
            .ThenSkip(OptionalLayout);

        private static readonly IParser<VmfElement> Element = Or(Lazy(() => Field), Lazy(() => Class));

        private static readonly IParser<VmfElement> Field = CommonParsingHelper.Field.Transform((n, v) => new VmfField(n, v));

        private static readonly IParser<VmfElement> Class
            = CompiledRegex("[a-zA-Z0-9_]+")
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("{"))
            .ThenSkip(OptionalLayout)
            .ThenAdd(Many(Element, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("}"))
            .Transform((n, c) => new VmfClass(n, c.Where(x => x is VmfField).Select(x => x as VmfField) !, c.Where(x => x is VmfClass).Select(x => x as VmfClass) !));

        private static readonly IParser<IEnumerable<IRadiantEntity>> Solids
            = OptionalLayout
            .Then(Many(Element, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .Transform(GetSolids)
            .Transform(x => x.Select(ToEntity));

        /// <summary>
        /// Parses a .vmf file.
        /// </summary>
        internal static readonly IParser<RadiantMap> Vmf
            = Solids
            .Transform(x => new RadiantMap(x))
            .ThenEnd();

        private static IRadiantEntity ToEntity(VmfClass c)
        {
            List<ClippingPlane> planes = new List<ClippingPlane>();

            foreach (VmfClass side in c.classes.Where(x => x.name == "side"))
            {
                string texture = side.fields.First(x => x.name == "material").value;
                string planeText = side.fields.First(x => x.name == "plane").value;
                (Vector v1, Vector v2, Vector v3) = Vertices.Parse(planeText);
                planes.Add(new ClippingPlane(v1, v2, v3, texture));
            }

            return new Brush(planes);
        }

        private static IEnumerable<VmfClass> GetSolids(IEnumerable<VmfElement> elements)
        {
            IEnumerable<VmfClass> classes = elements.Where(x => x is VmfClass).Select(x => x as VmfClass) !;
            IEnumerable<VmfClass> result = classes.Where(x => x.name == "solid") !;

            foreach (VmfClass c in result)
            {
                yield return c;
            }

            foreach (VmfClass c in classes)
            {
                foreach (VmfClass r in GetSolids(c.classes))
                {
                    yield return r;
                }
            }
        }
    }
}
