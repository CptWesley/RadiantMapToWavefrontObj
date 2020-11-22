using System;
using System.Collections.Generic;
using System.IO;

namespace RadiantMapToObj
{
    /// <summary>
    /// Represents a radiant map.
    /// </summary>
    public class RadiantMap
    {
        private List<Brush> brushes = new List<Brush>();
        private List<Patch> patches = new List<Patch>();

        /// <summary>
        /// Gets the brushes.
        /// </summary>
        public IEnumerable<Brush> Brushes => brushes;

        /// <summary>
        /// Gets the patches.
        /// </summary>
        public IEnumerable<Patch> Patches => patches;

        /// <summary>
        /// Parses a .map file to our radiant map object.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The parsed radiant map.</returns>
        public static RadiantMap Parse(string path)
        {
            string[] content = File.ReadAllLines(path);
            bool started = false;
            bool inBrush = false;
            bool inPatch = false;
            List<string>? brushLines = null;

            RadiantMap map = new RadiantMap();

            for (int i = 0; i < content.Length; ++i)
            {
                // Skip empty lines.
                if (content[i].Length < 1)
                {
                    continue;
                }

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
                        {
                            inPatch = true;
                        }
                        else
                        {
                            throw new ArgumentException("Not a proper .map file.");
                        }
                    }
                    else if (content[i].Contains("}"))
                    {
                        if (inPatch)
                        {
                            Patch? patch = Patch.CreateFromCode(brushLines!.ToArray());
                            if (patch != null)
                            {
                                map.Add(patch);
                            }

                            inPatch = false;
                            brushLines = new List<string>();
                        }
                        else if (inBrush)
                        {
                            Brush? brush = Brush.CreateFromCode(brushLines!.ToArray());
                            if (brush != null)
                            {
                                map.Add(brush);
                            }

                            inBrush = false;
                            brushLines = new List<string>();
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if (inBrush)
                    {
                        brushLines!.Add(content[i]);
                    }
                }
                else
                {
                    if (content[i][0] == '{')
                    {
                        started = true;
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// Adds a brush to the radiant map.
        /// </summary>
        /// <param name="brush">The brush.</param>
        public void Add(Brush brush)
            => brushes.Add(brush);

        /// <summary>
        /// Adds a patch to the radiant map.
        /// </summary>
        /// <param name="patch">The patch.</param>
        public void Add(Patch patch)
            => patches.Add(patch);

        /// <inheritdoc/>
        public override string ToString()
        {
            string res = string.Empty;

            for (int i = 0; i < brushes.Count; ++i)
            {
                res += "Brush " + i + ":\n";
                foreach (ClippingPlane f in brushes[i].ClippingPlanes)
                {
                    res += f + "\n";
                }
            }

            return res;
        }
    }
}
