using System.Collections.Generic;
using System.IO;
using System.Text;
using RadiantMapToObj.Internal.Conversion;
using RadiantMapToObj.Internal.Parsing;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Radiant
{
    /// <summary>
    /// Represents a radiant map.
    /// </summary>
    public class RadiantMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadiantMap"/> class.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public RadiantMap(IEnumerable<IRadiantEntity> entities)
            => Entities = entities;

        /// <summary>
        /// Gets the entities.
        /// </summary>
        public IEnumerable<IRadiantEntity> Entities { get; }

        /// <summary>
        /// Parses .map file formatted content to a map.
        /// </summary>
        /// <param name="content">The .map content.</param>
        /// <returns>The parsed radiant map.</returns>
        public static RadiantMap Parse(string content)
            => MapParsingHelper.Parse(content);

        /// <summary>
        /// Parses a .map file to our radiant map object.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The parsed radiant map.</returns>
        public static RadiantMap ParseFile(string path)
            => Parse(File.ReadAllText(path));

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach (IRadiantEntity entity in Entities)
            {
                sb.AppendLine($"Entity {i++}");
                sb.AppendLine(entity.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts the map to a <see cref="WavefrontObj"/> instance.
        /// </summary>
        /// <returns>A new <see cref="WavefrontObj"/> instance.</returns>
        public WavefrontObj ToObj()
            => MapConversionHelper.Convert(this);
    }
}
