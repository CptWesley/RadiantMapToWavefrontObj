using System.Collections.Generic;
using RadiantMapToObj.Configuration;
using RadiantMapToObj.Internal.Conversion;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Quake.Radiant
{
    /// <summary>
    /// Represents a radiant patch.
    /// </summary>
    public class Patch : IQuakeEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Patch"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public Patch(Grid<Vector> grid)
            => Grid = grid;

        /// <summary>
        /// Gets the grid.
        /// </summary>
        public Grid<Vector> Grid { get; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width => Grid.Width;

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height => Grid.Height;

        /// <summary>
        /// Gets all vertices.
        /// </summary>
        public IEnumerable<Vector> Vertices => Grid.Elements;

        /// <summary>
        /// Gets the <see cref="Vector"/> with at the specified x and y coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The vertex at the given coordinate.</returns>
        public Vector this[int x, int y] => Grid[x, y];

        /// <inheritdoc/>
        public ObjObject ToObjObject(ConversionSettings settings)
            => PatchConversionHelper.Convert(this, settings);
    }
}
