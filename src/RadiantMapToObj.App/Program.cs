using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using RadiantMapToObj.Radiant;
using RadiantMapToObj.Wavefront;

namespace RadiantMapToObj.App
{
    /// <summary>
    /// Entry class Program.
    /// </summary>
    internal static class Program
    {
        private static double scale = 0.01;
        private static bool autoclose;
        private static string[] textureFilter = Array.Empty<string>();

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version!;
            Console.WriteLine("RadiantMapToWavefrontObj version " + version.Major + '.' + version.Minor + '.' + version.Build);

            bool success = false;

            // Check for each argument if it is a .map we should convert.
            foreach (string arg in args)
            {
                if (File.Exists(arg) && Path.GetExtension(arg) == ".map")
                {
                    ConvertFile(arg);
                    success = true;
                }
                else
                {
                    HandleArgument(arg);
                }
            }

            if (!success)
            {
                Console.WriteLine("Invalid file.");
            }

            // Wait for console input before closing.
            Console.WriteLine("\nPress any key to close this window...");

            if (!autoclose)
            {
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Converts the file.
        /// </summary>
        /// <param name="path">The path.</param>
        private static void ConvertFile(string path)
        {
            Console.WriteLine("Parsing file: " + path + "...");

            DateTime startTime = DateTime.Now;

            RadiantMap map = RadiantMap.Parse(path);
            WavefrontObj obj = map.ToObj();

            if (textureFilter.Length > 0)
            {
                obj.FilterTextures(textureFilter);
            }

            obj.SaveFile(Path.Combine(Path.GetDirectoryName(path) !, Path.GetFileNameWithoutExtension(path)) + ".obj", scale);

            DateTime endTime = DateTime.Now;
            Console.WriteLine("Finished in: " + (endTime - startTime).Milliseconds + "ms.");
        }

        /// <summary>
        /// Handles the argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        private static void HandleArgument(string arg)
        {
            string pattern = @"-(\w+)=(\S+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = regex.Match(arg);
            if (m.Success)
            {
                string type = m.Groups[1].ToString();
                string mode = m.Groups[2].ToString();

                if (type == "autoclose")
                {
                    if (mode == "false" || mode == "0")
                    {
                        autoclose = false;
                    }
                    else if (mode == "true" || mode == "1")
                    {
                        autoclose = true;
                    }
                }
                else if (type == "scale")
                {
                    if (double.TryParse(mode, out double scale))
                    {
                        Program.scale = scale;
                    }
                }
                else if (type == "filter")
                {
                    if (File.Exists(mode))
                    {
                        textureFilter = File.ReadAllLines(mode);
                    }
                }
            }
        }
    }
}
