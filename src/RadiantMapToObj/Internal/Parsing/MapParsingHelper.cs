using System;
using System.Collections.Generic;
using System.IO;
using RadiantMapToObj.Radiant;

namespace RadiantMapToObj.Internal.Parsing
{
    /// <summary>
    /// Provides helper methods for parsing maps.
    /// </summary>
    internal static class MapParsingHelper
    {
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

            List<IRadiantEntity> entities = new List<IRadiantEntity>();

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
                            Patch patch = PatchParsingHelper.Parse(brushLines!.ToArray());
                            entities.Add(patch);

                            inPatch = false;
                            brushLines = new List<string>();
                        }
                        else if (inBrush)
                        {
                            Brush brush = BrushParsingHelper.Parse(brushLines!.ToArray());
                            entities.Add(brush);

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

            return new RadiantMap(entities);
        }
    }
}
