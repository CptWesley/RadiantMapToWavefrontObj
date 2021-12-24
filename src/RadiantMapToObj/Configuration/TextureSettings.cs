namespace RadiantMapToObj.Configuration
{
    /// <summary>
    /// Contains the information for texture settings.
    /// </summary>
    public class TextureSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether texture exporting is enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the search path for finding textures.
        /// </summary>
        public string SearchPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether an exact match for the texture should be found.
        /// </summary>
        public bool ExactMatch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether archives (zip files) should be included in the search.
        /// </summary>
        public bool IncludeArchives { get; set; }
    }
}
