using System;
using System.IO;

namespace PassGenPro.Generators
{
    public static class PhoneGenerator
    {
        public static void GenerateToFile(string prefix, string outputPath)
        {
            using var writer = new StreamWriter(outputPath, append: false);

            for (int i = 2000000; i <= 2999999; i++)
            {
                writer.WriteLine(prefix + i.ToString()); // 0502000000
            }

            Console.WriteLine($"Tamamlandı: {outputPath}");
        }

        public static void GenerateAll(string[] prefixes, string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            foreach (var prefix in prefixes)
            {
                var filePath = Path.Combine(outputDir, $"{prefix}.txt");
                Console.WriteLine($"Generasiya başladı: {prefix}...");
                GenerateToFile(prefix, filePath);
            }
        }
    }
}