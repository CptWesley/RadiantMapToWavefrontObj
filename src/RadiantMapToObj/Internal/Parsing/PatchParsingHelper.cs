using System;
using System.Globalization;
using System.Text.RegularExpressions;
using RadiantMapToObj.Radiant;

namespace RadiantMapToObj.Internal.Parsing
{
    /// <summary>
    /// Provides helper methods for parsing patches.
    /// </summary>
    internal static class PatchParsingHelper
    {
        /// <summary>
        /// Creates a radiant patch from a piece of code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>The created patch.</returns>
        internal static Patch Parse(string[] code)
        {
            if (code is null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            string sizePattern = @"(\s+)?\(\s?(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)(\s(\d+)\s(\d+))?\s?\)"; // width,height [2,3]
            Regex sizeRegex = new Regex(sizePattern, RegexOptions.IgnoreCase);

            int line = 0;

            Vector[][] grid = null!;
            while (line < code.Length)
            {
                Match m = sizeRegex.Match(code[line]);
                ++line;
                if (m.Success)
                {
                    int width = int.Parse(m.Groups[2].ToString(), CultureInfo.InvariantCulture);
                    int height = int.Parse(m.Groups[3].ToString(), CultureInfo.InvariantCulture);

                    grid = new Vector[width][];

                    for (int i = 0; i < grid.Length; ++i)
                    {
                        grid[i] = new Vector[height];
                    }

                    break;
                }
            }

            if (grid is null)
            {
                throw new ArgumentException("Incorrect patch format found.", nameof(code));
            }

            string num = @"-?\d+(\.\d+)?";
            string vertex = @"(\(\s?(" + num + @")\s(" + num + @")\s(" + num + @")\s(" // x,y,z [2,4,6]
                + num + @")\s(" + num + @")\s(" // patchDef2
                + "(" + num + @")\s(" + num + @")\s(" + num + @")\s(" + num + @")\s(" + num + @"))?\s?\))"; // patchDef3

            string pattern = vertex;

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            int x = 0;
            int y = 0;

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

                        grid[y][x] = v;

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

            return new Patch(grid);
        }
    }
}
