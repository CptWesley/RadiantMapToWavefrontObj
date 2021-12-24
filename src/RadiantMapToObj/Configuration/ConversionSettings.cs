namespace RadiantMapToObj.Configuration
{
    /// <summary>
    /// Represents the settings used during conversions.
    /// </summary>
    public class ConversionSettings
    {
        /// <summary>
        /// Gets or sets the filter settings.
        /// </summary>
        public Filter Filter { get; set; } = Filters.Empty;

        /// <summary>
        /// Gets or sets the texture settings.
        /// </summary>
        public TextureSettings Textures { get; set; } = new TextureSettings();

        /// <summary>
        /// Gets or sets a value indicating whether overlap of faces should be removed.
        /// </summary>
        public bool RemoveOverlappingFaces { get; set; }

        /// <summary>
        /// Gets or sets the scale to which models should be exported.
        /// </summary>
        public double Scale { get; set; } = 0.01;
    }
}
