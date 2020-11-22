using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RadiantMapToObj.Radiant;

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
        /// Converts a RadiantMap object to a WavefrontObj object.
        /// </summary>
        /// <param name="map">The radiant map to convert.</param>
        /// <returns>A wavefront object created from a given radiant map.</returns>
        public static WavefrontObj CreateFromRadiantMap(RadiantMap map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            List<ObjObject> objects = new List<ObjObject>();

            int i = 0;
            foreach (Brush brush in map.Brushes)
            {
                ObjObject obj = ObjObject.CreateFromBrush("Brush_" + i++, brush);
                objects.Add(obj);
            }

            foreach (Patch patch in map.Patches)
            {
                ObjObject obj = ObjObject.CreateFromPatch("Patch_" + i++, patch);
                objects.Add(obj);
            }

            return new WavefrontObj(objects.ToArray());
        }

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
            foreach (ObjObject obj in Objects)
            {
                res += obj.ToCode(scale, faceOffset) + "\n";
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
