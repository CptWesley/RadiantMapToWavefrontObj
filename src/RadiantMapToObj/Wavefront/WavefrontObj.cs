using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RadiantMapToObj.Configuration;

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
            Objects = objects.ToList();
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
        public void FilterTextures(Filter filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("# Exported using Wesley Baartman's RadiantMapToObj software.");
            sb.AppendLine("# https://github.com/CptWesley/RadiantMapToWavefrontObj");

            int faceOffset = 0;

            // Adds code for each object contained.
            int i = 0;
            foreach (ObjObject obj in Objects)
            {
                sb.AppendLine(obj.ToCode($"Object_{i++}", scale, faceOffset));
                faceOffset += obj.Vertices.Count();
            }

            return sb.ToString();
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
            => Objects = Objects.Where(obj => obj.Faces != null && obj.Faces.Any() && obj.Vertices.Any()).ToList();
    }
}
