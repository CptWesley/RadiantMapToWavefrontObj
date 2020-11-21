
namespace RadiantMapToWavefrontObj
{
    public static class ArrayExtension
    {
        // Adds an IndexOf method to arrays.
        public static int IndexOf<T>(this T[] arr, T other)
        {
            for (int i = 0; i < arr.Length; ++i)
            {
                if (arr[i].Equals(other))
                    return i;
            }
            return -1;
        }

        // Adds a Contains method to arrays.
        public static bool Contains<T>(this T[] arr, T other)
        {
            return IndexOf(arr, other) != -1;
        }
    }
}
