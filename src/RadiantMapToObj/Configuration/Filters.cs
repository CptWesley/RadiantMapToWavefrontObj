namespace RadiantMapToObj.Configuration
{
    /// <summary>
    /// Holds numerous pre-implemented filters.
    /// </summary>
    public static class Filters
    {
        /// <summary>
        /// Represents an empty filter.
        /// </summary>
        public static readonly Filter Empty = new Filter();

        /// <summary>
        /// Represents a filter for the hammer editor tools.
        /// </summary>
        public static readonly Filter Hammer = new Filter(new string[] { "TOOLS/*" });

        /// <summary>
        /// Represents a filter for the radiant editor utility textures.
        /// </summary>
        public static readonly Filter Radiant = new Filter(new string[] { "common/*" });

        /// <summary>
        /// Represents a filter for Enemy Territory maps.
        /// </summary>
        public static readonly Filter EnemyTerritory = new Filter(new string[] { "skies/*" }, new Filter[] { Radiant }, new[] { "common/terrain" });
    }
}
