using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RadiantMapToObj.Radiant
{
    /// <summary>
    /// Represents a radiant map.
    /// </summary>
    public class RadiantMap
    {
        private List<IRadiantEntity> entities = new List<IRadiantEntity>();

        /// <summary>
        /// Gets the entities.
        /// </summary>
        public IEnumerable<IRadiantEntity> Entities => entities;

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
                                map.entities.Add(patch);
                            }

                            inPatch = false;
                            brushLines = new List<string>();
                        }
                        else if (inBrush)
                        {
                            Brush? brush = Brush.CreateFromCode(brushLines!.ToArray());
                            if (brush != null)
                            {
                                map.entities.Add(brush);
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

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach (IRadiantEntity entity in entities)
            {
                sb.AppendLine($"Entity {i++}");
                sb.AppendLine(entity.ToString());
            }

            return sb.ToString();
        }
    }
}
