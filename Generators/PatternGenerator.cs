using System.Collections.Generic;
using PassGenPro.Utils;

namespace PassGenPro.Generators
{
    public static class PatternGenerator
    {
        // Most common real-world password patterns
        public static void Generate(HashSet<string> set, Config cfg)
        {
            var words = cfg.Words;
            var nums = cfg.Numbers;
            var syms = BaseGenerator.SYMBOLS;

            foreach (var w in words)
            {
                // Dot patterns: word.number, Word.Number
                if (cfg.UseDotPatterns)
                {
                    foreach (var n in nums)
                    {
                        set.Add(w + "." + n);
                        set.Add(WordUtils.Capitalize(w) + "." + n);
                        set.Add(w + "-" + n);
                        set.Add(WordUtils.Capitalize(w) + "-" + n);
                        set.Add(w + "_" + n);
                    }
                    foreach (var y in cfg.BirthYears)
                    {
                        set.Add(w + "." + y);
                        set.Add(WordUtils.Capitalize(w) + "." + y);
                        set.Add(WordUtils.Capitalize(w) + "-" + y);
                        // short year
                        if (y.Length == 4)
                        {
                            set.Add(w + "." + y.Substring(2));
                            set.Add(WordUtils.Capitalize(w) + "." + y.Substring(2));
                        }
                    }
                }

                // Symbol suffix patterns (most common: word+num+!)
                if (cfg.UseSymbols)
                {
                    foreach (var n in nums)
                        foreach (var s in syms)
                        {
                            set.Add(w + n + s);
                            set.Add(WordUtils.Capitalize(w) + n + s);
                            set.Add(w.ToUpper() + n + s);
                        }

                    foreach (var y in cfg.BirthYears)
                        foreach (var s in new[] { "!", "@", "#", "$", "_" })
                        {
                            set.Add(w + y + s);
                            set.Add(WordUtils.Capitalize(w) + y + s);
                        }
                }

                // Birth year combos
                foreach (var y in cfg.BirthYears)
                    BaseGenerator.AddWithSuffix(set, w, y, cfg);

                // All years if enabled
                if (cfg.UseYears)
                    foreach (var y in BaseGenerator.ALL_YEARS)
                        BaseGenerator.AddWithSuffix(set, w, y, cfg);

                // User numbers
                foreach (var n in nums)
                    BaseGenerator.AddWithSuffix(set, w, n, cfg);
            }

            // Word combos
            if (cfg.UseWordCombo)
            {
                for (int i = 0; i < words.Count; i++)
                for (int j = 0; j < words.Count; j++)
                {
                    if (i == j) continue;
                    var combo = words[i] + words[j];
                    BaseGenerator.AddVariants(set, combo, cfg);
                    foreach (var n in nums) BaseGenerator.AddWithSuffix(set, combo, n, cfg);
                    foreach (var y in cfg.BirthYears) BaseGenerator.AddWithSuffix(set, combo, y, cfg);
                    if (cfg.UseSymbols)
                        foreach (var s in new[] { ".", "_", "-", "@" })
                            set.Add(words[i] + s + words[j]);
                }
            }

            // Numbers alone with symbols
            foreach (var n in nums)
            {
                set.Add(n);
                if (cfg.UseSymbols) foreach (var s in syms) { set.Add(n + s); set.Add(s + n); }
            }
        }
    }
}