using System.Collections.Generic;
using System.IO;
using System.Linq;
using RadiantMapToObj.Configuration;

namespace RadiantMapToObj.Internal.TextureLoading
{
    /// <summary>
    /// Internal class for finding textures.
    /// </summary>
    internal static class TextureFinder
    {
        /// <summary>
        /// Finds the specified settings.
        /// </summary>
        /// <param name="settings">The settings for the texture search.</param>
        /// <param name="texture">The texture name.</param>
        /// <returns>The data stream of the texture.</returns>
        public static Stream? Find(TextureSettings settings, string texture)
            => FindInDirectory(settings, texture.Split('/', '\\'), new List<string>());

        private static Stream? FindInDirectory(TextureSettings settings, string[] texture, List<string> path)
        {
            string searchPath = string.IsNullOrWhiteSpace(settings.SearchPath) ? "./" : settings.SearchPath;
            string fullPath = Path.Combine(path.Prepend(searchPath).ToArray());
            DirectoryInfo di = new DirectoryInfo(fullPath);
            int missing = MissingMatchCount(texture, path);

            if (settings.ExactMatch && missing > 0 && missing == texture.Length - 1)
            {
                return null;
            }

            if (missing == 0)
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    if (Path.GetFileNameWithoutExtension(file.Name) == texture[texture.Length - 1])
                    {
                        return file.OpenRead();
                    }
                }
            }

            foreach (var subdir in di.GetDirectories())
            {
                List<string> newPath = new List<string>(path);
                newPath.Add(subdir.Name);
                Stream? result = FindInDirectory(settings, texture, newPath);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private static int MissingMatchCount(string[] texture, List<string> path)
        {
            int matched = 0;

            for (int i = texture.Length - 2; i >= 0; i--)
            {
                if (path.Count - matched - 1 < path.Count && path.Count - matched - 1 >= 0 && texture[i] == path[path.Count - matched - 1])
                {
                    matched++;
                }
            }

            return texture.Length - matched - 1;
        }
    }
}
