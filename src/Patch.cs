using System;
using System.Text.RegularExpressions;

namespace RadiantMapToWavefrontObj
{
    public class Patch
    {
        public Vertex[][] Grid { get; private set; }

        private int _x;
        private int _y;

        // Constructor for a patch.
        public Patch(int width, int height)
        {
            Grid = new Vertex[width][];

            for (int i = 0; i < Grid.Length; ++i)
            {
                Grid[i] = new Vertex[height];
                for (int j = 0; j < Grid[i].Length; ++j)
                    Grid[i][j] = null;
            }
        }

        public void Add(Vertex vertex)
        {
            Grid[_x][_y] = vertex;

            if (_x < Grid[0].Length-1)
                _x++;
            else
            {
                _x = 0;
                if (_y < Grid.Length - 1)
                    _y++;
                else
                    _y = 0;
            }
        }

        // Creates a radiant patch from a piece of code.
        public static Patch CreateFromCode(string[] code)
        {
            string sizePattern = @"(\s+)?\(\s?(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s(\d+)\s?\)";    //width,height [2,3]
            Regex sizeRegex = new Regex(sizePattern, RegexOptions.IgnoreCase);

            Patch patch = null;

            foreach (string line in code)
            {
                Match m = sizeRegex.Match(line);
                if (m.Success)
                    patch = new Patch(int.Parse(m.Groups[2].ToString()), int.Parse(m.Groups[3].ToString()));
            }

            string num = @"-?\d+(\.\d+)?";
            string vertex = @"(\(\s?(" + num + @")\s(" + num + @")\s(" + num + @")\s("      //x,y,z [2,4,6]
                + num + @")\s(" + num + @")\s(" + num + @")\s(" + num + @")\s("
                + num + @")\s(" + num + @")\s(" + num + @")\s?\))";

            string pattern = vertex;

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            foreach (string line in code)
            {
                MatchCollection m = regex.Matches(line);
                if (m.Count > 0)
                {
                    for (int i = 0; i < m.Count; ++i)
                    {
                        Vertex v = new Vertex(Double.Parse(m[i].Groups[2].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                            Double.Parse(m[i].Groups[4].ToString(), System.Globalization.CultureInfo.InvariantCulture),
                            Double.Parse(m[i].Groups[6].ToString(), System.Globalization.CultureInfo.InvariantCulture));

                        if (patch != null)
                            patch.Add(v);
                    }
                }
            }

            return patch;
        }
    }
}
