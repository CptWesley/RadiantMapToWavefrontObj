using System.Collections.Generic;
using System.Linq;

namespace RadiantMapToObj
{
    /// <summary>
    /// Represents a readonly 2 dimensional grid.
    /// </summary>
    /// <typeparam name="T">The type of elements in the grid.</typeparam>
    public class Grid<T>
    {
        private readonly T[][] grid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid{T}"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public Grid(T[][] grid)
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
        /// Gets all elements.
        /// </summary>
        public IEnumerable<T> Elements => grid.SelectMany(x => x);

        /// <summary>
        /// Gets the element at the specified x and y position.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <returns>The element at the given coordinate.</returns>
        public T this[int x, int y] => grid[y][x];
    }
}
