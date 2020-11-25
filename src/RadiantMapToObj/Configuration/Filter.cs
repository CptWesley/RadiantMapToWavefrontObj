using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RadiantMapToObj.Configuration
{
    /// <summary>
    /// Represents a texture filter.
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="textures">The filtered textures.</param>
        /// <param name="includes">The included filters.</param>
        /// <param name="ignores">The list of textures that are not filtered.</param>
        public Filter(IEnumerable<string> textures, IEnumerable<Filter> includes, IEnumerable<string> ignores)
            => (Textures, Includes, Ignores) = (textures, includes, ignores);

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="textures">The filtered textures.</param>
        /// <param name="includes">The included filters.</param>
        public Filter(IEnumerable<string> textures, IEnumerable<Filter> includes)
            : this(textures, includes, Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="textures">The filtered textures.</param>
        public Filter(IEnumerable<string> textures)
            : this(textures, Array.Empty<Filter>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="includes">The included filters.</param>
        public Filter(IEnumerable<Filter> includes)
            : this(Array.Empty<string>(), includes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        public Filter()
            : this(Array.Empty<string>())
        {
        }

        /// <summary>
        /// Gets the filtered textures.
        /// </summary>
        public IEnumerable<string> Textures { get; }

        /// <summary>
        /// Gets the included filters.
        /// </summary>
        public IEnumerable<Filter> Includes { get; }

        /// <summary>
        /// Gets the textures that are not filtered out.
        /// </summary>
        public IEnumerable<string> Ignores { get; }

        /// <summary>
        /// Tries to load a specific filter from the given fileName.
        /// If the file does not exist, try to load a pre-implemented filter.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The loaded filter.</returns>
        public static Filter Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                return new Filter(File.ReadAllLines(fileName));
            }

            return fileName?.ToUpperInvariant() switch
            {
                "ET" => Filters.EnemyTerritory,
                "RADIANT" => Filters.Radiant,
                "HAMMER" => Filters.Hammer,
                _ => throw new ArgumentException($"Could not find filter for '{fileName}'.", nameof(fileName)),
            };
        }

        /// <summary>
        /// Determines whether this filter contains a texture.
        /// </summary>
        /// <param name="texture">The texture to check.</param>
        /// <returns><c>true</c> if this filter contains the given texture; otherwise, <c>false</c>.</returns>
        public bool Contains(string texture)
        {
            if (IsIgnored(texture))
            {
                return false;
            }

            foreach (string pattern in Textures)
            {
                if (Matches(pattern, texture))
                {
                    return true;
                }
            }

            foreach (Filter include in Includes)
            {
                if (include.Contains(texture))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool Matches(string pattern, string texture)
        {
            string regexPattern = Regex.Escape(pattern).Replace("/", "\\/").Replace("\\*", ".*");
            return Regex.IsMatch(texture, regexPattern);
        }

        private bool IsIgnored(string texture)
        {
            foreach (string pattern in Ignores)
            {
                if (Matches(pattern, texture))
                {
                    return true;
                }
            }

            foreach (Filter include in Includes)
            {
                if (include.IsIgnored(texture))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
