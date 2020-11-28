using System;
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
        private readonly bool transposed;
        private readonly T[][] grid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid{T}"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public Grid(T[][] grid)
            : this(grid, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid{T}"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="transposed">Indicates whether or not the grid is transposed..</param>
        private Grid(T[][] grid, bool transposed)
        {
            this.grid = grid;
            this.transposed = transposed;
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width
            => transposed ? grid.Length : grid[0].Length;

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height
            => transposed ? grid[0].Length : grid.Length;

        /// <summary>
        /// Gets all elements.
        /// </summary>
        public IEnumerable<T> Elements => grid.SelectMany(x => x);

        /// <summary>
        /// Gets the transpose.
        /// </summary>
        public Grid<T> Transpose => new Grid<T>(grid, !transposed);

        /// <summary>
        /// Gets the element at the specified x and y position.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <returns>The element at the given coordinate.</returns>
        public T this[int x, int y]
            => transposed ? grid[x][y] : grid[y][x];

        /// <inheritdoc/>
        public override string ToString()
            => string.Join(Environment.NewLine, grid.Select(x => string.Join(" ", x)));
    }
}
