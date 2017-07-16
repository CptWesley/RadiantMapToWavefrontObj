using System.Collections.Generic;
using System.IO;

namespace RadiantMapToWavefrontObj
{
    public class RadiantMap
    {
        public readonly Brush[] Brushes;

        // Constructor that creates a radiant map object.
        public RadiantMap(Brush[] brushes)
        {
            Brushes = brushes;
        }

        // Returns a string format of the entire radiant map.
        public override string ToString()
        {
            string res = "";

            for (int i = 0; i < Brushes.Length; ++i)
            {
                res += "Brush " + i + ":\n";
                foreach (ClippingPlane f in Brushes[i].ClippingPlanes)
                    res += f + "\n";
            }

            return res;
        }

        // Parses a .map file to our radiant map object.
        public static RadiantMap Parse(string path)
        {
            string[] content = File.ReadAllLines(path);
            bool started = false;
            bool inBrush = false;
            List<string> brushLines = null;
            List<Brush> brushes = new List<Brush>();

            for (int i = 0; i < content.Length; ++i)
            {
                // Skip empty lines.
                if (content[i].Length < 1)
                    continue;

                if (started)
                {
                    if (content[i][0] == '{')
                    {
                        if (!inBrush)
                        {
                            inBrush = true;
                            brushLines = new List<string>();
                        }
                        else
                            return null;
                    }
                    else if (content[i][0] == '}')
                    {
                        if (inBrush)
                        {
                            brushes.Add(Brush.CreateFromCode(brushLines.ToArray()));
                            inBrush = false;
                            brushLines = new List<string>();
                        }
                        else
                            break;
                    }
                    else if (inBrush)
                        brushLines.Add(content[i]);
                }
                else
                {
                    if (content[i][0] == '{')
                        started = true;
                }

            }

            return new RadiantMap(brushes.ToArray());
        }
    }
}
