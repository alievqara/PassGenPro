using System.Collections.Generic;
using PassGenPro.Utils;

namespace PassGenPro.Generators
{
    public static class EssidGenerator
    {
        public static void Generate(HashSet<string> set, Config cfg)
        {
            if (string.IsNullOrWhiteSpace(cfg.Essid)) return;
            var e = cfg.Essid.Trim();

            // Basic variants
            BaseGenerator.AddVariants(set, e, cfg);

            // ESSID + numbers + years
            foreach (var n in cfg.Numbers) BaseGenerator.AddWithSuffix(set, e, n, cfg);
            foreach (var y in cfg.BirthYears) BaseGenerator.AddWithSuffix(set, e, y, cfg);
            if (cfg.UseYears)
                foreach (var y in BaseGenerator.ALL_YEARS)
                {
                    set.Add(e + y);
                    set.Add(WordUtils.Capitalize(e) + y);
                    set.Add(e + y + "!");
                }

            // ESSID + user words/names
            foreach (var w in cfg.Words) { set.Add(e + w); set.Add(w + e); }
            foreach (var n in cfg.Names) { set.Add(e + n); set.Add(n + e); set.Add(WordUtils.Capitalize(n) + e); }

            // Common router default passwords based on ESSID
            var essidLower = e.ToLower();
            var essidCap = WordUtils.Capitalize(e);
            set.Add(essidLower + "123");
            set.Add(essidLower + "1234");
            set.Add(essidLower + "12345");
            set.Add(essidLower + "123456");
            set.Add(essidCap + "123");
            set.Add(essidCap + "1234");
            set.Add(essidCap + "123!");
            set.Add(essidCap + "2024");
            set.Add(essidCap + "2023");
            set.Add(e + "@123");
            set.Add(e + "#123");
            set.Add(e + "wifi");
            set.Add(e + "Wifi");
            set.Add(e + "WIFI");
            set.Add("wifi" + e);
            set.Add(essidLower + "password");
            set.Add(essidCap + "Password");
            set.Add(e + "admin");
            set.Add(e + "Admin");

            // Leet
            if (cfg.UseLeet)
            {
                var l = WordUtils.Leet(e);
                set.Add(l + "123"); set.Add(l + "1234"); set.Add(l + "!");
            }
        }
    }
}