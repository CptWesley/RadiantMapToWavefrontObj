using System;
using System.IO;
using System.Reflection;

namespace RadiantMapToWavefrontObj
{
    internal class Program
    {
        private static double _scale = 0.01;
        private static bool _autoclose = false;

        static void Main(string[] args)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine("RadiantMapToWavefrontObj version " + version.Major + '.' + version.Minor + '.' + version.Build);

            // Check if the file exists.
            if (args.Length > 0 && File.Exists(args[0]))
                ConvertFile(args[0]);
            else
                Console.WriteLine("Invalid file.");

            // Wait for console input before closing.
            Console.WriteLine("\nPress any key to close this window...");

            if (!_autoclose)
                Console.ReadKey();
        }

        // Convert .map file to .obj file.
        private static void ConvertFile(string path)
        {
            Console.WriteLine("Parsing file: " + path + "...");

            DateTime startTime = DateTime.Now;

            RadiantMap map = RadiantMap.Parse(path);
            WavefrontObj obj = WavefrontObj.CreateFromRadiantMap(map);

            obj.SaveFile(Path.GetFileNameWithoutExtension(path) + ".obj", _scale);

            DateTime endTime = DateTime.Now;
            Console.WriteLine("Finished in: " + (endTime-startTime).Milliseconds + "ms.");
        }
    }
}
