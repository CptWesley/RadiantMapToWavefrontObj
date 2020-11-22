using System;

namespace RadiantMapToWavefrontObj
{
    /// <summary>
    /// Extension class for <see cref="Array"/>.
    /// </summary>
    public static class ArrayExtension
    {
        /// <summary>
        /// Get index of element.
        /// </summary>
        /// <typeparam name="T">Type of element to be found.</typeparam>
        /// <param name="arr">The arr.</param>
        /// <param name="element">The other.</param>
        /// <returns>Index of element.</returns>
        public static int IndexOf<T>(this T[] arr, T element)
            => Array.IndexOf(arr, element);
    }
}
