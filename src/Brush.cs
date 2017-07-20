using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RadiantMapToWavefrontObj
{
    public class Brush
    {
        public readonly ClippingPlane[] ClippingPlanes;

        // Constructor for radiant brushes.
        public Brush(ClippingPlane[] clippingPlanes)
        {
            ClippingPlanes = clippingPlanes;
        }

        // Creates a radiant brush from a piece of code.
        public static Brush CreateFromCode(string[] code)
        {
            ClippingPlane[] planes = CreateClippingPlanes(code);

            return new Brush(planes);
        }

        // Creates the needed clipping planes based on code.
        private static ClippingPlane[] CreateClippingPlanes(string[] code)
        {
            string pattern = @"(\(\s?-?\d+(\.\d+)?\s-?\d+(\.\d+)?\s-?\d+(\.\d+)?\s?\))\s?"  // First vertex [1]
                + @"(\(\s?-?\d+(\.\d+)?\s-?\d+(\.\d+)?\s-?\d+(\.\d+)?\s?\))\s?"             // Second vertex [5]
                + @"(\(\s?-?\d+(\.\d+)?\s-?\d+(\.\d+)?\s-?\d+(\.\d+)?\s?\))\s"              // Third vertex [9]
                + @"(\w+(\/\S*)*)"                                                          // Texture [13]
                + @".*";                                                                    // Leftovers
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            List<ClippingPlane> planes = new List<ClippingPlane>();

            foreach (string line in code)
            {
                Match m = regex.Match(line);
                if (m.Success)
                {
                    Vertex v1 = Vertex.CreateFromCode(m.Groups[1].ToString());
                    Vertex v2 = Vertex.CreateFromCode(m.Groups[5].ToString());
                    Vertex v3 = Vertex.CreateFromCode(m.Groups[9].ToString());

                    planes.Add(new ClippingPlane(v1, v2, v3, m.Groups[13].ToString()));
                }
            }

            return planes.ToArray();
        }
    }
}
