using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RadiantMapToObj.Quake;
using RadiantMapToObj.Quake.Hammer;
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
        internal abstract record VmfElement;

        [SuppressMessage("Spacing Rules", "SA1009", Justification = "Contradictory rules.")]
        internal record VmfField(string Name, string Value) : VmfElement;

        [SuppressMessage("Spacing Rules", "SA1009", Justification = "Contradictory rules.")]
        internal record VmfClass(string Name, IEnumerable<VmfField> Fields, IEnumerable<VmfClass> Classes) : VmfElement;

        private static readonly IParser<(Vector, Vector, Vector)> Vertices
            = OptionalLayout
            .Then(Multiple(Vertex, OptionalLayout, 3))
            .ThenSkip(OptionalLayout)
            .Transform(x => (x[0], x[1], x[2]));

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
            .Transform((n, c) => new VmfClass(n, c.Where(x => x is VmfField).Select(x => x as VmfField)!, c.Where(x => x is VmfClass).Select(x => x as VmfClass)!));

        private static readonly IParser<IEnumerable<IQuakeEntity>> Solids
            = OptionalLayout
            .Then(Many(Element, OptionalLayout))
            .ThenSkip(OptionalLayout)
            .Transform(GetSolids)
            .Transform(x => x.Select(ToEntity));

        /// <summary>
        /// Parses a .vmf file.
        /// </summary>
        internal static readonly IParser<QuakeMap> Vmf
            = Solids
            .Transform(x => new QuakeMap(x))
            .ThenEnd();

        private static IQuakeEntity ToEntity(VmfClass c)
        {
            List<ClippingPlane> planes = new List<ClippingPlane>();

            foreach (VmfClass side in c.Classes.Where(x => x.Name == "side"))
            {
                string texture = side.Fields.First(x => x.Name == "material").Value;
                string planeText = side.Fields.First(x => x.Name == "plane").Value;
                (Vector v1, Vector v2, Vector v3) = Vertices.Parse(planeText);

                VmfClass? dispInfo = side.Classes.FirstOrDefault(x => x.Name == "dispinfo");
                if (dispInfo != null)
                {
                    planes.Add(new DisplacementClippingPlane(v1, v2, v3, texture, DisplacementParsingHelper.ToDispInfo(dispInfo)));
                }
                else
                {
                    planes.Add(new ClippingPlane(v1, v2, v3, texture));
                }
            }

            return new Brush(planes);
        }

        private static IEnumerable<VmfClass> GetSolids(IEnumerable<VmfElement> elements)
        {
            IEnumerable<VmfClass> classes = elements.Where(x => x is VmfClass).Select(x => x as VmfClass)!;
            IEnumerable<VmfClass> result = classes.Where(x => x.Name == "solid")!;

            foreach (VmfClass c in result)
            {
                yield return c;
            }

            foreach (VmfClass c in classes)
            {
                foreach (VmfClass r in GetSolids(c.Classes))
                {
                    yield return r;
                }
            }
        }
    }
}
