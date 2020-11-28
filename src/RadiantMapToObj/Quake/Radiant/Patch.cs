using System.Collections.Generic;
using System.Linq;
using RadiantMapToObj.Internal.Conversion;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.Quake.Radiant
{
    /// <summary>
    /// Represents a radiant patch.
    /// </summary>
    public class Patch : IQuakeEntity
    {
        private Vector[][] grid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Patch"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public Patch(Vector[][] grid)
            => this.grid = grid;

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width => grid[0].Length;

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height => grid.Length;

        /// <summary>
        /// Gets all vertices.
        /// </summary>
        public IEnumerable<Vector> Vertices => grid.SelectMany(x => x);

        /// <summary>
        /// Gets the <see cref="Vector"/> with at the specified x and y coordinates.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The vertex at the given coordinate.</returns>
        public Vector this[int x, int y] => grid[y][x];

        /// <inheritdoc/>
        public ObjObject ToObjObject()
            => PatchConversionHelper.Convert(this);
    }
}
