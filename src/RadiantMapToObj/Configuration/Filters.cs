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
        public static readonly Filter Hammer = new Filter(new string[]
        {
            "TOOLS/*",
        });
    }
}
