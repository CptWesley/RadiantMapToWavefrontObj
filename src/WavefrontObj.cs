using System.Collections.Generic;
using System.IO;

namespace RadiantMapToWavefrontObj
{
    public class WavefrontObj
    {
        public readonly ObjObject[] Objects;

        // Constructor for an entire wavefront .obj file object.
        public WavefrontObj(ObjObject[] objects)
        {
            Objects = objects;
        }

        // Converts a RadiantMap object to a WavefrontObj object.
        public static WavefrontObj CreateFromRadiantMap(RadiantMap map)
        {
            List<ObjObject> objects = new List<ObjObject>();

            for (int i = 0; i < map.Brushes.Length; ++i)
            {
                Brush brush = map.Brushes[i];
                ObjObject obj = ObjObject.CreateFromBrush("Brush_" + i, brush);
                objects.Add(obj);
            }

            return new WavefrontObj(objects.ToArray());
        }

        // Returns .obj formatted text of this object.
        public string ToCode(double scale, string[] ignores)
        {
            string res = "# Exported using Wesley Baartman's RadiantMapToWavefrontObj software.\n\n";

            int faceOffset = 0;

            // Adds code for each object contained.
            foreach (ObjObject obj in Objects)
            {
                res += obj.ToCode(scale, faceOffset, ignores) + "\n";
                faceOffset += obj.Vertices.Length;
            }

            return res;
        }

        // Overload that does not require ignores.
        public string ToCode(double scale)
        {
            return ToCode(scale, new string[0]);
        }

        // Overload that does not require ignores or scale.
        public string ToCode()
        {
            return ToCode(1, new string[0]);
        }

        // Saves this object to an .obj file.
        public void SaveFile(string path, double scale, string[] ignores)
        {
            File.WriteAllText(path, ToCode(scale, ignores));
        }

        // Overload that does not require ignores.
        public void SaveFile(string path, double scale)
        {
            File.WriteAllText(path, ToCode(scale));
        }

        // Overload that does not require ignores or scale.
        public void SaveFile(string path)
        {
            File.WriteAllText(path, ToCode());
        }
    }
}
