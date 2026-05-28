using System;
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
                BaseGenerator.AddVariants(set, name, cfg);

                foreach (var y in birthYears)
                {
                    set.Add(name + y);
                    set.Add(WordUtils.Capitalize(name) + y);
                    set.Add(name.ToUpper() + y);
                    if (y.Length >= 2)
                    {
                        set.Add(name + y.Substring(y.Length - 2));
                        set.Add(WordUtils.Capitalize(name) + y.Substring(y.Length - 2));
                    }
                    foreach (var s in syms)
                    {
                        set.Add(WordUtils.Capitalize(name) + y + s);
                        set.Add(name + y + s);
                    }
                    set.Add(WordUtils.Capitalize(name) + "." + y);
                    set.Add(name + "." + y);
                }

                foreach (var n in nums)
                    BaseGenerator.AddWithSuffix(set, name, n, cfg);

                if (cfg.UseYears)
                    foreach (var y in BaseGenerator.ALL_YEARS)
                    {
                        set.Add(name + y);
                        set.Add(WordUtils.Capitalize(name) + y);
                        set.Add(WordUtils.Capitalize(name) + y + "!");
                        set.Add(WordUtils.Capitalize(name) + y + "@");
                    }

                foreach (var sur in surnames)
                {
                    set.Add(name + sur);
                    set.Add(sur + name);
                    set.Add(WordUtils.Capitalize(name) + WordUtils.Capitalize(sur));
                    set.Add(WordUtils.Capitalize(sur) + WordUtils.Capitalize(name));
                    if (name.Length > 0 && sur.Length > 0)
                    {
                        set.Add(name[0] + sur);
                        set.Add(char.ToUpper(name[0]) + WordUtils.Capitalize(sur));
                        set.Add(WordUtils.Capitalize(name) + sur[0].ToString().ToUpper());
                        set.Add(name + "." + sur);
                        set.Add(sur + "." + name);
                        set.Add(name + "_" + sur);
                    }
                    foreach (var y in birthYears)
                    {
                        set.Add(WordUtils.Capitalize(name) + WordUtils.Capitalize(sur) + y);
                        set.Add(WordUtils.Capitalize(sur) + WordUtils.Capitalize(name) + y);
                        set.Add(WordUtils.Capitalize(name) + y + WordUtils.Capitalize(sur));
                    }
                }

                foreach (var w in cfg.Words)
                {
                    set.Add(name + w);
                    set.Add(w + name);
                    set.Add(WordUtils.Capitalize(name) + WordUtils.Capitalize(w));
                }
            }

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
    }
}