using System.Collections.Generic;
using PassGenPro.Utils;

namespace PassGenPro.Generators
{
    public static class NameGenerator
    {
        public static void Generate(HashSet<string> set, Config cfg)
        {
            var names = cfg.Names;
            var surnames = cfg.Surnames;
            var birthYears = cfg.BirthYears;
            var nums = cfg.Numbers;
            var syms = new[] { "!", "@", "#", ".", "_", "-", "$" };

            foreach (var name in names)
            {
                // Basic variants
                BaseGenerator.AddVariants(set, name, cfg);

                // Name + birth year (Metehan2018 pattern)
                foreach (var y in birthYears)
                {
                    set.Add(name + y);
                    set.Add(WordUtils.Capitalize(name) + y);
                    set.Add(name.ToUpper() + y);
                    set.Add(name + y.Substring(Math.Max(0, y.Length - 2))); // short year
                    set.Add(WordUtils.Capitalize(name) + y.Substring(Math.Max(0, y.Length - 2)));
                    // With symbols
                    foreach (var s in syms)
                    {
                        set.Add(WordUtils.Capitalize(name) + y + s);
                        set.Add(name + y + s);
                    }
                    // Dot pattern
                    set.Add(WordUtils.Capitalize(name) + "." + y);
                    set.Add(name + "." + y);
                }

                // Name + numbers
                foreach (var n in nums)
                    BaseGenerator.AddWithSuffix(set, name, n, cfg);

                // Name + all years
                if (cfg.UseYears)
                    foreach (var y in BaseGenerator.ALL_YEARS)
                    {
                        set.Add(name + y);
                        set.Add(WordUtils.Capitalize(name) + y);
                        set.Add(WordUtils.Capitalize(name) + y + "!");
                        set.Add(WordUtils.Capitalize(name) + y + "@");
                    }

                // Name + surname combos
                foreach (var sur in surnames)
                {
                    set.Add(name + sur);
                    set.Add(sur + name);
                    set.Add(WordUtils.Capitalize(name) + WordUtils.Capitalize(sur));
                    set.Add(WordUtils.Capitalize(sur) + WordUtils.Capitalize(name));
                    // initials
                    if (name.Length > 0 && sur.Length > 0)
                    {
                        set.Add(name[0] + sur);
                        set.Add(char.ToUpper(name[0]) + WordUtils.Capitalize(sur));
                        set.Add(WordUtils.Capitalize(name) + sur[0].ToString().ToUpper());
                        set.Add(name + "." + sur);
                        set.Add(sur + "." + name);
                        set.Add(name + "_" + sur);
                    }
                    // With birth year
                    foreach (var y in birthYears)
                    {
                        set.Add(WordUtils.Capitalize(name) + WordUtils.Capitalize(sur) + y);
                        set.Add(WordUtils.Capitalize(sur) + WordUtils.Capitalize(name) + y);
                        set.Add(WordUtils.Capitalize(name) + y + WordUtils.Capitalize(sur));
                    }
                }

                // Name + user words
                foreach (var w in cfg.Words)
                {
                    set.Add(name + w);
                    set.Add(w + name);
                    set.Add(WordUtils.Capitalize(name) + WordUtils.Capitalize(w));
                }
            }

            // Surname alone variants
            foreach (var sur in surnames)
            {
                BaseGenerator.AddVariants(set, sur, cfg);
                foreach (var y in birthYears)
                {
                    set.Add(sur + y);
                    set.Add(WordUtils.Capitalize(sur) + y);
                    set.Add(WordUtils.Capitalize(sur) + y + "!");
                }
                foreach (var n in nums)
                    BaseGenerator.AddWithSuffix(set, sur, n, cfg);
            }
        }

        static int Math_Max(int a, int b) => a > b ? a : b;
    }

    file static class Math
    {
        public static int Max(int a, int b) => a > b ? a : b;
    }
}