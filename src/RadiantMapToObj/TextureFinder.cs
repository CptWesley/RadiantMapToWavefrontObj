using System.Collections.Generic;
using RadiantMapToObj.Configuration;
using RadiantMapToObj.Internal.TextureLoading;

namespace RadiantMapToObj
{
    /// <summary>
    /// Class responsible for finding textures.
    /// </summary>
    public class TextureFinder
    {
        private Dictionary<string, (int, int, string)> savedValues = new Dictionary<string, (int, int, string)>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureFinder"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public TextureFinder(TextureSettings settings)
            => Settings = settings;

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public TextureSettings Settings { get; set; }

        /// <summary>
        /// Finds the size of the given texture.
        /// </summary>
        /// <param name="texture">The texture name.</param>
        /// <returns>The size of the texture.</returns>
        public (int Width, int Height) FindSize(string texture)
        {
            if (savedValues.TryGetValue(texture, out (int, int, string) value))
            {
                return (value.Item1, value.Item2);
            }

            value = TextureFinderHelper.Find(Settings, texture);
            savedValues.Add(texture, value);
            return (value.Item1, value.Item2);
        }

        /// <summary>
        /// Finds the extension of the given texture.
        /// </summary>
        /// <param name="texture">The texture name.</param>
        /// <returns>The extension of the texture.</returns>
        public string FindExtension(string texture)
        {
            if (savedValues.TryGetValue(texture, out (int, int, string) value))
            {
                return value.Item3;
            }

            value = TextureFinderHelper.Find(Settings, texture);
            savedValues.Add(texture, value);
            return value.Item3;
        }
    }
}
