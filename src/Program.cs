using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RadiantMapToWavefrontObj
{
    internal class Program
    {
        private static double _scale = 0.01;
        private static string[] _textureFilter = new string[0];

        static void Main(string[] args)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
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
                    HandleArgument(arg);
            }

            if (!success)
                Console.Error.WriteLine("Invalid file.");
                Environment.Exit(1);

        }

        // Convert .map file to .obj file.
        private static void ConvertFile(string path)
        {
            Console.WriteLine("Parsing file: " + path + "...");

            DateTime startTime = DateTime.Now;

            RadiantMap map = RadiantMap.Parse(path);
            WavefrontObj obj = WavefrontObj.CreateFromRadiantMap(map);

            if (_textureFilter.Length > 0)
                obj.FilterTextures(_textureFilter);

            obj.SaveFile(Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)) + ".obj", _scale);

            DateTime endTime = DateTime.Now;
            Console.WriteLine("Finished in: " + (endTime-startTime).Milliseconds + "ms.");
        }

        // Handle a settings argument.
        private static void HandleArgument(string arg)
        {
            string pattern = @"-(\w+)=(\S+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match m = regex.Match(arg);
            if (m.Success)
            {
                string type = m.Groups[1].ToString();
                string mode = m.Groups[2].ToString();

                if (type == "scale")
                {
                    double scale;
                    if (Double.TryParse(mode, out scale))
                        _scale = scale;
                }
                else if (type == "filter")
                {
                    if (File.Exists(mode))
                        _textureFilter = File.ReadAllLines(mode);
                }
            }
        }
    }
}
