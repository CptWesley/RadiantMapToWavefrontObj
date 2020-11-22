using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using RadiantMapToObj.Internal;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Radiant
{
    /// <summary>
    /// Represents a radiant patch.
    /// </summary>
    public class Patch : IRadiantEntity
    {
        private Vector[][] grid;
        private int x;
        private int y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Patch"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public Patch(Vector[][] grid)
            => this.grid = grid;

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width => grid[0].Length;

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height => grid.Length;

        /// <summary>
        /// Gets all vertices.
        /// </summary>
        public IEnumerable<Vector> Vertices => grid.SelectMany(x => x);

        /// <summary>
        /// Gets the <see cref="Vector"/> with at the specified x and y coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The vertex at the given coordinate.</returns>
        public Vector this[int x, int y] => grid[x][y];

        /// <summary>
        /// Creates a radiant patch from a piece of code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>The created patch.</returns>
        public static Patch? CreateFromCode(string[] code)
        {
            if (code is null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            string sizePattern = @"(\s+)?\(\s?(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)(\s(\d+)\s(\d+))?\s?\)"; // width,height [2,3]
            Regex sizeRegex = new Regex(sizePattern, RegexOptions.IgnoreCase);

            Patch? patch = null;

            int line = 0;

            while (line < code.Length)
            {
                Match m = sizeRegex.Match(code[line]);
                ++line;
                if (m.Success)
                {
                    int width = int.Parse(m.Groups[2].ToString(), CultureInfo.InvariantCulture);
                    int height = int.Parse(m.Groups[3].ToString(), CultureInfo.InvariantCulture);

                    Vector[][] grid = new Vector[width][];

                    for (int i = 0; i < grid.Length; ++i)
                    {
                        grid[i] = new Vector[height];
                    }

                    patch = new Patch(grid);
                    break;
                }
            }

            string num = @"-?\d+(\.\d+)?";
            string vertex = @"(\(\s?(" + num + @")\s(" + num + @")\s(" + num + @")\s(" // x,y,z [2,4,6]
                + num + @")\s(" + num + @")\s(" // patchDef2
                + "(" + num + @")\s(" + num + @")\s(" + num + @")\s(" + num + @")\s(" + num + @"))?\s?\))"; // patchDef3

            string pattern = vertex;

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            while (line < code.Length)
            {
                MatchCollection m = regex.Matches(code[line]);
                ++line;
                if (m.Count > 0)
                {
                    for (int i = 0; i < m.Count; ++i)
                    {
                        Vector v = new Vector(
                            -double.Parse(m[i].Groups[2].ToString(), CultureInfo.InvariantCulture),
                            -double.Parse(m[i].Groups[4].ToString(), CultureInfo.InvariantCulture),
                            -double.Parse(m[i].Groups[6].ToString(), CultureInfo.InvariantCulture));

                        if (patch != null)
                        {
                            patch.Add(v);
                        }
                    }
                }
            }

            return patch;
        }

        /// <inheritdoc/>
        public ObjObject ToObjObject(string name)
            => PatchConversionHelper.Convert(this, name);

        /// <summary>
        /// Adds a vertex to the grid in the next available slot.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        private void Add(Vector vertex)
        {
            grid[y][x] = vertex;

            if (x < grid[0].Length - 1)
            {
                x++;
            }
            else
            {
                x = 0;
                if (y < grid.Length - 1)
                {
                    y++;
                }
                else
                {
                    y = 0;
                }
            }
        }
    }
}
