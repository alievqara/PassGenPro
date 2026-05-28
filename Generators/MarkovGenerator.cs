using System;
using System.Collections.Generic;
using System.Text;
using PassGenPro.Utils;

namespace PassGenPro.Generators
{
    public static class MarkovGenerator
    {
        // Pre-trained bigram frequencies from real passwords
        static readonly Dictionary<string, string[]> Bigrams = new()
        {
            {"pa", new[]{"ss","sw","th","rt","ck"}},
            {"ss", new[]{"wo","wa","10","20","12"}},
            {"sw", new[]{"or","ar","ee","12","19"}},
            {"wo", new[]{"rd","rk","rt","rs","r1"}},
            {"rd", new[]{"s","1","12","!","s1"}},
            {"qu", new[]{"er","ic","al","ee","ik"}},
            {"er", new[]{"ty","to","12","20","s"}},
            {"ty", new[]{"12","1","ui","!","@"}},
            {"ad", new[]{"mi","ob","am","12","1"}},
            {"mi", new[]{"n","ch","ke","ss","nd"}},
            {"lo", new[]{"ve","gi","ok","ck","ad"}},
            {"ve", new[]{"r","s","12","!","20"}},
            {"ma", new[]{"st","in","ry","ss","n"}},
            {"st", new[]{"er","ar","12","1","!"}},
            {"su", new[]{"mm","ns","pe","n","b"}},
            {"mm", new[]{"er","it","12","y","on"}},
            {"ba", new[]{"ku","ck","ll","by","se"}},
            {"ku", new[]{"12","!","ba","sh","@"}},
            {"dr", new[]{"ag","iv","ew","12","op"}},
            {"ag", new[]{"on","e","12","!","in"}},
        };

        static readonly string[] SEEDS = {
            "pa","sw","qu","ad","lo","ma","su","ba","dr","mi",
            "se","sh","st","sp","tr","cr","gr","br","pr","cl"
        };

        public static void Generate(HashSet<string> set, Config cfg)
        {
            var rng = new Random(42);
            int count = 0;
            int target = 50000;

            // Seed from user words
            var seedWords = new List<string>(cfg.Words);
            seedWords.AddRange(cfg.Names);
            seedWords.AddRange(cfg.Surnames);

            foreach (var seed in seedWords)
            {
                if (seed.Length < 2) continue;
                for (int attempt = 0; attempt < 20; attempt++)
                {
                    var word = GenerateFromSeed(seed.Substring(0, Math.Min(2, seed.Length)).ToLower(), rng, 4, 8);
                    if (word.Length >= 4)
                    {
                        set.Add(word);
                        set.Add(WordUtils.Capitalize(word));
                        // Add with years
                        foreach (var y in cfg.BirthYears) set.Add(WordUtils.Capitalize(word) + y);
                        foreach (var n in cfg.Numbers) set.Add(word + n);
                        count++;
                    }
                }
            }

            // Pure Markov chains
            while (count < target)
            {
                var seed = SEEDS[rng.Next(SEEDS.Length)];
                var word = GenerateFromSeed(seed, rng, 5, 9);
                if (word.Length >= 5)
                {
                    set.Add(word);
                    set.Add(WordUtils.Capitalize(word));
                    if (cfg.UseYears && cfg.BirthYears.Count > 0)
                        set.Add(WordUtils.Capitalize(word) + cfg.BirthYears[rng.Next(cfg.BirthYears.Count)]);
                    count++;
                }
            }
        }

        static string GenerateFromSeed(string seed, Random rng, int minLen, int maxLen)
        {
            var sb = new StringBuilder(seed);
            int maxIter = maxLen * 2;
            while (sb.Length < maxLen && maxIter-- > 0)
            {
                if (sb.Length < 2) break;
                var key = sb.ToString().Substring(sb.Length - 2);
                if (Bigrams.TryGetValue(key, out var nexts))
                {
                    var next = nexts[rng.Next(nexts.Length)];
                    sb.Append(next);
                }
                else break;
            }
            var result = sb.ToString();
            return result.Length >= minLen ? result.Substring(0, Math.Min(result.Length, maxLen)) : "";
        }
    }
}