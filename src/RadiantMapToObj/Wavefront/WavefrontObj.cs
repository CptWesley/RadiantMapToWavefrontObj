using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RadiantMapToObj.Wavefront
{
    /// <summary>
    /// Represents a wavefront obj file.
    /// </summary>
    public class WavefrontObj
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WavefrontObj"/> class.
        /// </summary>
        /// <param name="objects">The objects.</param>
        public WavefrontObj(IEnumerable<ObjObject> objects)
        {
            Objects = objects;
            Cleanup();
        }

        /// <summary>
        /// Gets the objects.
        /// </summary>
        public IEnumerable<ObjObject> Objects { get; private set; }

        /// <summary>
        /// Removes all faces containing a texture listed in the filter from all subobjects.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public void FilterTextures(string[] filter)
        {
            foreach (ObjObject obj in Objects)
            {
                obj.FilterTextures(filter);
            }

            Cleanup();
        }

        /// <summary>
        /// Converts the object to .obj file content.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <returns>The object represented in .obj file content format.</returns>
        public string ToCode(double scale)
        {
            string res = "# Exported using Wesley Baartman's RadiantMapToWavefrontObj software.\n\n";

            int faceOffset = 0;

            // Adds code for each object contained.
            int i = 0;
            foreach (ObjObject obj in Objects)
            {
                res += obj.ToCode($"Object_{i++}", scale, faceOffset) + "\n";
                faceOffset += obj.Vertices.Count();
            }

            return res;
        }

        /// <summary>
        /// Saves this object to an .obj file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="scale">The scale.</param>
        public void SaveFile(string path, double scale)
            => File.WriteAllText(path, ToCode(scale));

        /// <summary>
        /// Removes objects that lack faces or vertices.
        /// </summary>
        private void Cleanup()
            => Objects = Objects.Where(obj => obj.Faces != null && obj.Faces.Any() && obj.Vertices.Any());
    }
}
