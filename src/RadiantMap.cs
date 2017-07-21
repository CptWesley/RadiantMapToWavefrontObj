using System;
using System.Collections.Generic;
using System.IO;

namespace RadiantMapToWavefrontObj
{
    public class RadiantMap
    {
        private List<Brush> _brushes;
        private List<Patch> _patches;
        public Brush[] Brushes => _brushes.ToArray();
        public Patch[] Patches => _patches.ToArray();

        // Constructor that creates a radiant map object.
        public RadiantMap()
        {
            _brushes = new List<Brush>();
            _patches = new List<Patch>();
        }

        // Adds a brush to the radiant map.
        public void Add(Brush brush)
        {
            _brushes.Add(brush);
        }

        // Adds a patch to the radiant map.
        public void Add(Patch patch)
        {
            _patches.Add(patch);
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
            bool inPatch = false;
            List<string> brushLines = null;
            List<Brush> brushes = new List<Brush>();
            List<Patch> patches = new List<Patch>();

            for (int i = 0; i < content.Length; ++i)
            {
                // Skip empty lines.
                if (content[i].Length < 1)
                    continue;

                if (started)
                {
                    if (content[i].Contains("{"))
                    {
                        if (!inBrush)
                        {
                            inBrush = true;
                            brushLines = new List<string>();
                            brushLines.Add(content[i]);
                        }
                        else if (!inPatch)
                            inPatch = true;
                        else
                            return null;
                    }
                    else if (content[i].Contains("}"))
                    {
                        if (inPatch)
                        {
                            patches.Add(Patch.CreateFromCode(brushLines.ToArray()));
                            inPatch = false;
                        }
                        else if (inBrush)
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
