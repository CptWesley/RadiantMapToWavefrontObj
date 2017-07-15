using System;
using System.IO;
using System.Reflection;

namespace RadiantMapToWavefrontObj
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("RadiantMapToWavefrontObj version " + Assembly.GetExecutingAssembly().GetName().Version);

            // Check if the file exists.
            if (args.Length > 0 && File.Exists(args[0]))
                ConvertFile(args[0]);
            else
                Console.WriteLine("Invalid file.");

            // Wait for console input before closing.
            Console.WriteLine("\nPress any key to close this window...");
            Console.ReadKey();
        }

        // Convert .map file to .obj file.
        private static void ConvertFile(string path)
        {
            Console.WriteLine("Parsing file: " + path + "...");

            DateTime startTime = DateTime.Now;

            RadiantMap map = RadiantMap.Parse(path);
            WavefrontObj obj = WavefrontObj.CreateFromRadiantMap(map);

            obj.SaveFile(Path.GetFileNameWithoutExtension(path) + ".obj", 0.01);

            DateTime endTime = DateTime.Now;
            Console.WriteLine("Finished in: " + (endTime-startTime).Milliseconds + "ms.");
        }
    }
}
