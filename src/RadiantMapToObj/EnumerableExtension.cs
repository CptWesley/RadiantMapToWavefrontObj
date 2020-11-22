using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RadiantMapToObj
{
    /// <summary>
    /// Extension class for <see cref="IEnumerable"/>.
    /// </summary>
    internal static class EnumerableExtension
    {
        /// <summary>
        /// Get index of element.
        /// </summary>
        /// <typeparam name="T">Type of element to be found.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="element">The element.</param>
        /// <returns>Index of element.</returns>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T element)
        {
            int i = 0;
            foreach (T val in enumerable)
            {
                if ((val is null && element is null) || (val != null && val.Equals(element)))
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        /// <summary>
        /// Gets the element at the given index.
        /// </summary>
        /// <typeparam name="T">Type of element to be found.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="index">The index.</param>
        /// <returns>The element at the given index.</returns>
        public static T Get<T>(this IEnumerable<T> enumerable, int index)
            => enumerable.Skip(index).First();

        /// <summary>
        /// Checks that the length of an enumerable is at least a given count.
        /// </summary>
        /// <typeparam name="T">Type of element in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="count">The count.</param>
        /// <returns>True if count is at least the given value, false otherwise.</returns>
        public static bool CountAtLeast<T>(this IEnumerable<T> enumerable, int count)
        {
            int i = 0;
            foreach (T element in enumerable)
            {
                if (++i == count)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
