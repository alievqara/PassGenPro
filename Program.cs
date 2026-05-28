using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using PassGenPro.Generators;
using PassGenPro.UI;
using PassGenPro.Utils;

namespace PassGenPro
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            RunMainMenu();
        }

        static void RunMainMenu()
        {
            while (true)
            {
                TUI.Banner();
                TUI.SectionHeader("ANA MENYU");

                int choice = TUI.ArrowMenu(new[]
                {
                    "🚀  Yeni Wordlist Yarat",
                    "⚙️   Ayarlar / Konfiqurasiya",
                    "📖  Haqqında",
                    "❌  Çıx"
                });

                switch (choice)
                {
                    case 0: RunGenerator(); break;
                    case 1: RunSettings(); break;
                    case 2: RunAbout(); break;
                    case 3: return;
                }
            }
        }

        static Config CollectInput()
        {
            var cfg = new Config();
            TUI.Banner();
            TUI.SectionHeader("1 / 5  —  AÇAR SÖZLƏR");
            TUI.Dim("Hədəf haqqında bildiklərin: ev adı, şəhər, sevimli şey...");
            TUI.Dim("Boş sətir ilə bitir.");
            Console.WriteLine();
            cfg.Words = TUI.PromptList("Sözlər", "baku, wifi, home, ...");

            TUI.Banner();
            TUI.SectionHeader("2 / 5  —  AD / SOYAD");
            TUI.Dim("Hədəfin adı / soyadı (varsa)");
            Console.WriteLine();
            cfg.Names = TUI.PromptList("Adlar", "Metehan, Aysel, ...");
            cfg.Surnames = TUI.PromptList("Soyadlar", "Yilmaz, Aliyev, ...");

            TUI.Banner();
            TUI.SectionHeader("3 / 5  —  RƏQƏMLƏR & İLLƏR");
            TUI.Dim("Doğum ili, əhəmiyyətli rəqəmlər, tarixlər...");
            Console.WriteLine();
            cfg.BirthYears = TUI.PromptList("Doğum illəri", "1990, 1995, ...");
            cfg.Numbers = TUI.PromptList("Digər rəqəmlər", "123, 786, 007, ...");

            TUI.Banner();
            TUI.SectionHeader("4 / 5  —  TELEFON & WiFi");
            Console.WriteLine();
            cfg.Phones = TUI.PromptList("Telefon nömrələri", "0501234567, ...");
            cfg.Essid = TUI.Prompt("WiFi adı (ESSID)", "");

            TUI.Banner();
            TUI.SectionHeader("5 / 5  —  KOMBİNASİYA QAYDALAR");
            TUI.Dim("SPACE ilə seç/ləğv et, A=hamısı, N=heç biri");
            Console.WriteLine();

            string[] ruleNames = {
                "Leet speak        (a→@ e→3 o→0 s→$ t→7)",
                "Böyük/kiçik       (Word WORD wOrD ...)",
                "Xüsusi simvollar  (! @ # $ _ . ...)",
                "İl əlavəsi        (1970-2025 bütün illər)",
                "Söz+söz           (bakuwifi, wifibaku ...)",
                "Daxili baza       (rockyou top sözlər)",
                "Tərsinə           (baku → ukab)",
                "Təkrarlama        (bakubaku, BAKUBAKU)",
                "Keyboard walks    (qwerty, asdf, 1q2w3e ...)",
                "Ad/Soyad pattern  (Metehan2018, M.Yilmaz ...)",
                "Telefon pattern   (0501234567, +994...)",
                "ESSID pattern     (WiFi adından kombinasiya)",
                "Markov zənciri    (real şifrəyə bənzər sözlər)",
                "Nöqtə patternlər  (word.2018, Name.18 ...)",
                "Hybrid mode       (baza+qaydalar birlikdə)",
            };

            bool[] defaults = Enumerable.Repeat(true, ruleNames.Length).ToArray();
            bool[] selected = TUI.CheckboxMenu(ruleNames, defaults);

            cfg.UseLeet          = selected[0];
            cfg.UseCaseVariants  = selected[1];
            cfg.UseSymbols       = selected[2];
            cfg.UseYears         = selected[3];
            cfg.UseWordCombo     = selected[4];
            cfg.UseBaseWords     = selected[5];
            cfg.UseReverse       = selected[6];
            cfg.UseDuplicate     = selected[7];
            cfg.UseKeyboardWalks = selected[8];
            cfg.UseNamePatterns  = selected[9];
            cfg.UsePhonePatterns = selected[10];
            cfg.UseEssidPatterns = selected[11];
            cfg.UseMarkov        = selected[12];
            cfg.UseDotPatterns   = selected[13];
            cfg.UseHybrid        = selected[14];

            TUI.Banner();
            TUI.SectionHeader("ÇIXIŞ AYARLARI");
            cfg.OutputFile = TUI.Prompt("Fayl adı", "wordlist.txt");
            if (!cfg.OutputFile.EndsWith(".txt")) cfg.OutputFile += ".txt";
            var minStr = TUI.Prompt("Minimum şifrə uzunluğu", "8");
            var maxStr = TUI.Prompt("Maksimum şifrə uzunluğu", "32");
            cfg.MinLength = int.TryParse(minStr, out int mn) ? mn : 8;
            cfg.MaxLength = int.TryParse(maxStr, out int mx) ? mx : 32;

            return cfg;
        }

        static void RunGenerator()
        {
            var cfg = CollectInput();

            TUI.Banner();
            TUI.SectionHeader("YARADILIR...");

            var set = new HashSet<string>(StringComparer.Ordinal);
            var sw = Stopwatch.StartNew();
            int step = 0, totalSteps = 8;

            void Step(string name, Action action)
            {
                step++;
                TUI.ProgressBar(step - 1, totalSteps, $"[{step}/{totalSteps}] {name}");
                action();
                TUI.ProgressBar(step, totalSteps, $"[{step}/{totalSteps}] {name} ✓   {set.Count:N0} şifrə");
                Console.WriteLine();
            }

            // 1. Base word variants
            Step("Söz variantları", () =>
            {
                foreach (var w in cfg.Words) BaseGenerator.AddVariants(set, w, cfg);
            });

            // 2. Patterns (word+num+sym combos)
            Step("Pattern kombinasiyaları", () => PatternGenerator.Generate(set, cfg));

            // 3. Name patterns
            if (cfg.UseNamePatterns)
                Step("Ad/Soyad patternləri", () => NameGenerator.Generate(set, cfg));
            else step++;

            // 4. Keyboard walks
            if (cfg.UseKeyboardWalks)
                Step("Keyboard walks", () => KeyboardGenerator.Generate(set, cfg));
            else step++;

            // 5. Phone patterns
            if (cfg.UsePhonePatterns)
                Step("Telefon patternləri", () => PhoneGenerator.Generate(set, cfg));
            else step++;

            // 6. ESSID patterns
            if (cfg.UseEssidPatterns && !string.IsNullOrWhiteSpace(cfg.Essid))
                Step("ESSID patternləri", () => EssidGenerator.Generate(set, cfg));
            else step++;

            // 7. Base wordlist
            if (cfg.UseBaseWords)
                Step("Daxili baza", () =>
                {
                    foreach (var bw in BaseGenerator.BASE_WORDS)
                    {
                        set.Add(bw);
                        BaseGenerator.AddVariants(set, bw, cfg);
                        if (cfg.UseSymbols) foreach (var s in new[] { "!", "@", "#" }) set.Add(bw + s);
                        foreach (var n in cfg.Numbers.Take(5)) BaseGenerator.AddWithSuffix(set, bw, n, cfg);
                        if (cfg.UseYears) foreach (var y in BaseGenerator.ALL_YEARS.Take(10)) set.Add(bw + y);
                        foreach (var uw in cfg.Words) { set.Add(bw + uw); set.Add(uw + bw); }
                        foreach (var nm in cfg.Names) { set.Add(bw + nm); set.Add(nm + bw); }
                    }
                });
            else step++;

            // 8. Markov
            if (cfg.UseMarkov)
                Step("Markov zənciri", () => MarkovGenerator.Generate(set, cfg));
            else step++;

            Console.WriteLine();

            // Filter by length
            TUI.Warn($"Uzunluq filtrəsi tətbiq edilir ({cfg.MinLength}-{cfg.MaxLength})...");
            var final = set.Where(p => p.Length >= cfg.MinLength && p.Length <= cfg.MaxLength).ToList();

            // Write file
            TUI.Warn($"Fayla yazılır: {cfg.OutputFile}");
            string passwordsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Passwords");
            Directory.CreateDirectory(passwordsDir);
            string homePath = Path.Combine(Path.GetFullPath(passwordsDir), cfg.OutputFile);

            using var writer = new StreamWriter(homePath, false, Encoding.UTF8, 65536);
            long written = 0;
            foreach (var p in final)
            {
                writer.WriteLine(p);
                written++;
                if (written % 500000 == 0)
                    TUI.ProgressBar(written, final.Count, $"{written:N0} yazıldı");
            }
            TUI.ProgressBar(final.Count, final.Count, $"{final.Count:N0} yazıldı");
            Console.WriteLine();

            sw.Stop();
            var fi = new FileInfo(homePath);

            TUI.Banner();
            TUI.SectionHeader("NƏTİCƏ");
            TUI.Stat("Cəmi şifrə:",     $"{final.Count:N0}", ConsoleColor.Green);
            TUI.Stat("Fayl ölçüsü:",    WordUtils.FormatSize(fi.Length), ConsoleColor.Cyan);
            TUI.Stat("Fayl yolu:",       homePath, ConsoleColor.White);
            TUI.Stat("Vaxt:",            $"{sw.Elapsed.TotalSeconds:F1} saniyə", ConsoleColor.Yellow);
            TUI.Stat("Sürət:",           $"{(long)(final.Count / sw.Elapsed.TotalSeconds):N0} şifrə/san", ConsoleColor.Cyan);
            Console.WriteLine();
            TUI.Success("Wordlist hazırdır! aircrack-ng ilə istifadə et:");
            TUI.Dim($"aircrack-ng capture.cap -w {homePath}");
            Console.WriteLine();
            TUI.PressAny();
        }

        static void RunSettings()
        {
            TUI.Banner();
            TUI.SectionHeader("AYARLAR");
            TUI.Dim("Bu versiyada ayarlar hər session üçün ayrıca seçilir.");
            TUI.Dim("Növbəti versiyada konfiqurasiya faylı saxlama olacaq.");
            Console.WriteLine();
            TUI.PressAny();
        }

        static void RunAbout()
        {
            TUI.Banner();
            TUI.SectionHeader("HAQQINDA");
            TUI.Stat("Proqram:",   "PassGen PRO v2.0");
            TUI.Stat("Dil:",       "C# / .NET 6");
            TUI.Stat("Məqsəd:",    "WPA/WPA2 pentest wordlist generatoru");
            TUI.Stat("Qaydalar:",  "15 kombinasiya qaydası");
            TUI.Stat("Generasiya:","1M - 10M+ şifrə");
            Console.WriteLine();
            TUI.Dim("Yalnız öz şəbəkəni test etmək üçün istifadə et!");
            Console.WriteLine();
            TUI.PressAny();
        }
    }
}