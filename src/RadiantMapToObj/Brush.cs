using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RadiantMapToWavefrontObj
{
    /// <summary>
    /// Class for Brush.
    /// </summary>
    public class Brush
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Brush"/> class.
        /// </summary>
        /// <param name="clippingPlanes">The clipping planes.</param>
        public Brush(IEnumerable<ClippingPlane> clippingPlanes)
            => ClippingPlanes = clippingPlanes;

        /// <summary>
        /// Gets the clipping planes.
        /// </summary>
        public IEnumerable<ClippingPlane> ClippingPlanes { get; }

        /// <summary>
        /// Creates from code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>The new Brush object generated from code.</returns>
        public static Brush CreateFromCode(string[] code)
        {
            if (code is null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            IEnumerable<ClippingPlane> planes = CreateClippingPlanes(code);
            return new Brush(planes);
        }

        /// <summary>
        /// Creates the clipping planes.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>An IEnumerable of Clipping Planes.</returns>
        private static IEnumerable<ClippingPlane> CreateClippingPlanes(string[] code)
        {
            // TODO: rewrite parser
            string num = @"-?\d+(\.\d+)?";
            string vertex = @"(\(\s?" + num + @"\s" + num + @"\s" + num + @"\s?\))";
            string pattern = vertex + @"\s?" // First vertex [1]
                             + vertex + @"\s?" // Second vertex [5]
                             + vertex + @"\s" // Third vertex [9]
                             + @"(\w+(\/\S*)*)" // Texture [13]
                             + @".*"; // Leftovers
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

            return planes;
        }
    }
}
