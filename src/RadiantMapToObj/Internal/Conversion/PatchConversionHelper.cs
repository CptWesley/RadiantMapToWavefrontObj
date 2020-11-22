using System.Collections.Generic;
using RadiantMapToObj.Radiant;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Internal.Conversion
{
    /// <summary>
    /// Provides helper functions for converting patches to objects.
    /// </summary>
    internal static class PatchConversionHelper
    {
        /// <summary>
        /// Converts a radiant patch to an obj object.
        /// </summary>
        /// <param name="patch">The patch.</param>
        /// <param name="name">The name.</param>
        /// <returns>A newly created obj object.</returns>
        internal static ObjObject Convert(Patch patch, string name)
        {
            IEnumerable<Vector> vertices = patch.Vertices;
            IEnumerable<Face> faces = CreateFaces(patch);
            return new ObjObject(name, vertices, faces);
        }

        /// <summary>
        /// Creates faces from a patch.
        /// </summary>
        /// <param name="patch">The patch.</param>
        /// <returns>The faces that fill the grid.</returns>
        private static IEnumerable<Face> CreateFaces(Patch patch)
        {
            List<Face> faces = new List<Face>();

            for (int x = 0; x < patch.Width - 1; ++x)
            {
                for (int y = 0; y < patch.Height - 1; ++y)
                {
                    faces.Add(new Face(patch[x, y], patch[x + 1, y], patch[x, y + 1], string.Empty));
                    faces.Add(new Face(patch[x, y + 1], patch[x + 1, y], patch[x + 1, y + 1], string.Empty));
                }
            }

            return faces;
        }
    }
}
