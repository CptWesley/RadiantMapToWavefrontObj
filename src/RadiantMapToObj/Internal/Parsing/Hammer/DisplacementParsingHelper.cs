using System.Globalization;
using System.Linq;
using RadiantMapToObj.Quake.Hammer;
using Warpstone;
using static RadiantMapToObj.Internal.Parsing.CommonParsingHelper;
using static RadiantMapToObj.Internal.Parsing.Hammer.VmfParsingHelper;
using static Warpstone.Parsers.BasicParsers;

namespace RadiantMapToObj.Internal.Parsing.Hammer
{
    /// <summary>
    /// Provides functionality for parsing displacements.
    /// </summary>
    internal static class DisplacementParsingHelper
    {
        private static readonly IParser<Vector> StartPos
            = OptionalLayout
            .ThenSkip(String("["))
            .ThenSkip(OptionalLayout)
            .Then(Multiple(Double, Layout, 3))
            .ThenSkip(OptionalLayout)
            .ThenSkip(String("]"))
            .ThenSkip(OptionalLayout)
            .Transform(x => new Vector(x[0], x[1], x[2]));

        private static readonly IParser<double[]> DoubleRow = Multiple(Double, Layout, 5, 17).Transform(x => x.ToArray());
        private static readonly IParser<Vector> SingleVector = Multiple(Double, Layout, 3).Transform(x => new Vector(x[0], x[1], x[2]));
        private static readonly IParser<Vector[]> VectorRow = Multiple(SingleVector, Layout, 5, 17).Transform(x => x.ToArray());

        /// <summary>
        /// Converts a vmf class to a displacement.
        /// </summary>
        /// <param name="c">The class.</param>
        /// <returns>The displacement info.</returns>
        internal static DisplacementInfo ToDispInfo(VmfClass c)
        {
            int power = int.Parse(c.Fields.First(x => x.Name == "power").Value, CultureInfo.InvariantCulture);
            int dimensions = PowerToDimensions(power);
            Vector startPos = StartPos.Parse(c.Fields.First(x => x.Name == "startposition").Value);
            double elevation = Double.Parse(c.Fields.First(x => x.Name == "elevation").Value);
            Grid<Vector> normals = GetGrid(VectorRow, c.Classes.First(x => x.Name == "normals"), dimensions);
            Grid<double> distances = GetGrid(DoubleRow, c.Classes.First(x => x.Name == "distances"), dimensions);
            Grid<Vector> offsets = GetGrid(VectorRow, c.Classes.First(x => x.Name == "offsets"), dimensions);
            Grid<Vector> offsetNormals = GetGrid(VectorRow, c.Classes.First(x => x.Name == "offset_normals"), dimensions);

            return new DisplacementInfo(dimensions, startPos, elevation, normals, distances, offsets, offsetNormals);
        }

        private static int PowerToDimensions(int power)
            => power switch
            {
                2 => 5,
                3 => 9,
                _ => 17,
            };

        private static Grid<T> GetGrid<T>(IParser<T[]> rowParser, VmfClass c, int dimensions)
        {
            T[][] result = new T[dimensions][];

            for (int i = 0; i < dimensions; i++)
            {
                string value = c.Fields.First(x => x.Name == $"row{i}").Value;
                result[i] = rowParser.Parse(value);
            }

            return new Grid<T>(result);
        }
    }
}
