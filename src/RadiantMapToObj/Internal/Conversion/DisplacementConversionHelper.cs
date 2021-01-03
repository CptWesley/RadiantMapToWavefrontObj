using System;
using System.Collections.Generic;
using System.Linq;
using RadiantMapToObj.Quake.Hammer;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Internal.Conversion
{
    /// <summary>
    /// Provides conversion methods for displacements.
    /// </summary>
    internal static class DisplacementConversionHelper
    {
        /// <summary>
        /// Converts a hammer displacement to an obj object.
        /// </summary>
        /// <param name="displacementPlane">The clipping plane containing the displacement.</param>
        /// <param name="vertices">The vertices of the brush.</param>
        /// <returns>A newly created obj object.</returns>
        public static ObjObject Convert(DisplacementClippingPlane displacementPlane, IEnumerable<Vector> vertices)
        {
            Vector[] verticesInPlane = vertices.Where(x => x.OnPlane(displacementPlane)).ToArray();

            if (verticesInPlane.Length != 4)
            {
                Console.WriteLine("Error - Not enough vertices for displacement.");
                return new ObjObject(Array.Empty<Vector>(), Array.Empty<Face>());
            }

            DisplacementInfo dispInfo = displacementPlane.DisplacementInfo;

            Grid<Vector> grid = CreateGrid(verticesInPlane, dispInfo.Dimensions);
            grid = ApplyOffsets(grid, dispInfo.Offsets);
            grid = ApplyDistances(grid, dispInfo.Normals, dispInfo.Elevation, dispInfo.Distances);
            return ObjFromGrid(grid, displacementPlane.Texture.Name);
        }

        private static Grid<Vector> CreateGrid(Vector[] vertices, int dimensions)
        {
            Vector a = vertices[3];
            Vector b = vertices[0];
            Vector c = vertices[2];
            Vector d = vertices[1];

            Vector[] left = CreateRow(a, c, dimensions);
            Vector[] right = CreateRow(d, b, dimensions);

            Vector[][] grid = new Vector[dimensions][];

            for (int i = 0; i < dimensions; i++)
            {
                grid[i] = CreateRow(left[i], right[i], dimensions);
            }

            return new Grid<Vector>(grid);
        }

        private static Vector[] CreateRow(Vector a, Vector b, int dimensions)
        {
            Vector[] result = new Vector[dimensions];

            Vector movement = b - a;
            Vector normal = movement.Unit;
            double spacing = movement.Length / (dimensions - 1);

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = a + (normal * spacing * i);
            }

            return result;
        }

        private static Grid<Vector> ApplyOffsets(Grid<Vector> grid, Grid<Vector> offsets)
        {
            Vector[][] newGrid = new Vector[grid.Height][];

            for (int y = 0; y < grid.Height; y++)
            {
                newGrid[y] = new Vector[grid.Width];
                for (int x = 0; x < grid.Width; x++)
                {
                    newGrid[y][x] = grid[x, y] - offsets[grid.Width - 1 - x, y];
                }
            }

            return new Grid<Vector>(newGrid);
        }

        private static Grid<Vector> ApplyDistances(Grid<Vector> grid, Grid<Vector> normals, double elevation, Grid<double> distances)
        {
            Vector[][] newGrid = new Vector[grid.Height][];

            for (int y = 0; y < grid.Height; y++)
            {
                newGrid[y] = new Vector[grid.Width];
                for (int x = 0; x < grid.Width; x++)
                {
                    newGrid[y][x] = grid[x, y] - (normals[grid.Width - 1 - x, y] * (elevation + distances[grid.Width - 1 - x, y]));
                }
            }

            return new Grid<Vector>(newGrid);
        }

        private static ObjObject ObjFromGrid(Grid<Vector> grid, string texture)
        {
            List<Face> faces = new List<Face>();

            for (int x = 0; x < grid.Width - 1; ++x)
            {
                for (int y = 0; y < grid.Height - 1; ++y)
                {
                    faces.Add(new Face(grid[x, y], grid[x + 1, y], grid[x, y + 1], texture));
                    faces.Add(new Face(grid[x, y + 1], grid[x + 1, y], grid[x + 1, y + 1], texture));
                }
            }

            return new ObjObject(grid.Elements, faces);
        }
    }
}
