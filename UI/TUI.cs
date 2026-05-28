using System;
using System.Collections.Generic;

namespace PassGenPro.UI
{
    public static class TUI
    {
        public static void Clear() => Console.Clear();

        public static void Banner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine("  ────────────────────────────────────────────");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("          Dobby Password Generator");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("          v1.3  |  by Dobby");
            Console.WriteLine("  ────────────────────────────────────────────");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void SectionHeader(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("  ╔"); Console.Write(new string('═', title.Length + 2)); Console.WriteLine("╗");
            Console.Write("  ║ "); Console.ForegroundColor = ConsoleColor.Cyan; Console.Write(title);
            Console.ForegroundColor = ConsoleColor.DarkCyan; Console.WriteLine(" ║");
            Console.Write("  ╚"); Console.Write(new string('═', title.Length + 2)); Console.WriteLine("╝");
            Console.ResetColor(); Console.WriteLine();
        }

        public static void Success(string msg) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"  ✓  {msg}"); Console.ResetColor(); }
        public static void Warn(string msg) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"  ⚡ {msg}"); Console.ResetColor(); }
        public static void Error(string msg) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"  ✗  {msg}"); Console.ResetColor(); }
        public static void Dim(string msg) { Console.ForegroundColor = ConsoleColor.DarkGray; Console.WriteLine($"     {msg}"); Console.ResetColor(); }

        public static string Prompt(string label, string def = "")
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan; Console.Write($"  ▸ {label}");
            if (!string.IsNullOrEmpty(def)) { Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($" [{def}]"); }
            Console.ForegroundColor = ConsoleColor.White; Console.Write(": "); Console.ResetColor();
            var r = Console.ReadLine()?.Trim() ?? "";
            return string.IsNullOrEmpty(r) ? def : r;
        }

        public static List<string> PromptList(string label, string hint = "")
        {
            var list = new List<string>();
            Console.ForegroundColor = ConsoleColor.DarkCyan; Console.WriteLine($"  ▸ {label}");
            if (!string.IsNullOrEmpty(hint)) { Console.ForegroundColor = ConsoleColor.DarkGray; Console.WriteLine($"    ({hint})"); }
            Console.ResetColor();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($"    [{list.Count + 1}] ");
                Console.ForegroundColor = ConsoleColor.White;
                var v = Console.ReadLine()?.Trim() ?? ""; Console.ResetColor();
                if (string.IsNullOrEmpty(v)) break;
                list.Add(v);
            }
            return list;
        }

        public static int ArrowMenu(string[] options, string? title = null)
        {
            int selected = 0;
            Console.CursorVisible = false;

            void Draw()
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  ╔══════════════════════════════════════╗");
                if (title != null)
                {
                    Console.Write("  ║  ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{title,-36}");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("║");
                    Console.WriteLine("  ╠══════════════════════════════════════╣");
                }

                for (int i = 0; i < options.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("  ║  ");
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("▶  ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.Write($"{options[i],-33}");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("  ║");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("   ");
                        Console.Write($"{options[i],-33}");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("  ║");
                    }
                }

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("  ╚══════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("    ↑↓ hərəkət   ENTER seç");
                Console.ResetColor();
            }

            Draw();

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                int lines = options.Length + (title != null ? 4 : 3) + 1;

                if (key == ConsoleKey.UpArrow) selected = (selected - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.DownArrow) selected = (selected + 1) % options.Length;
                else if (key == ConsoleKey.Enter) break;

                Console.SetCursorPosition(0, Console.CursorTop - lines);
                Draw();
            }

            Console.CursorVisible = true;
            Console.WriteLine();
            return selected;
        }

        public static bool[] CheckboxMenu(string[] options, bool[]? defaults = null)
        {
            bool[] selected = new bool[options.Length];
            if (defaults != null) for (int i = 0; i < Math.Min(defaults.Length, selected.Length); i++) selected[i] = defaults[i];
            int cursor = 0; Console.CursorVisible = false;
            void Draw()
            {
                Console.ForegroundColor = ConsoleColor.DarkGray; Console.WriteLine("  (↑↓ hərəkət  |  SPACE seç  |  A=hamı  |  N=heç biri  |  ENTER bitir)\n"); Console.ResetColor();
                for (int i = 0; i < options.Length; i++)
                {
                    string check = selected[i] ? "◉" : "○";
                    if (i == cursor) { Console.ForegroundColor = ConsoleColor.Cyan; Console.Write($"  ❯ "); Console.ForegroundColor = selected[i] ? ConsoleColor.Cyan : ConsoleColor.DarkGray; Console.Write($"{check} "); Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(options[i]); Console.ResetColor(); }
                    else { Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($"    {check} "); Console.ForegroundColor = selected[i] ? ConsoleColor.White : ConsoleColor.DarkGray; Console.WriteLine(options[i]); Console.ResetColor(); }
                }
            }
            Draw();
            while (true)
            {
                var key = Console.ReadKey(true).Key; int lines = options.Length + 2;
                if (key == ConsoleKey.UpArrow) cursor = (cursor - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.DownArrow) cursor = (cursor + 1) % options.Length;
                else if (key == ConsoleKey.Spacebar) selected[cursor] = !selected[cursor];
                else if (key == ConsoleKey.Enter) break;
                else if (key == ConsoleKey.A) for (int i = 0; i < selected.Length; i++) selected[i] = true;
                else if (key == ConsoleKey.N) for (int i = 0; i < selected.Length; i++) selected[i] = false;
                Console.SetCursorPosition(0, Console.CursorTop - lines);
                Draw();
            }
            Console.CursorVisible = true; Console.WriteLine(); return selected;
        }

        public static void ProgressBar(long current, long total, string extra = "", int barWidth = 48)
        {
            double pct = total == 0 ? 1 : (double)current / total;
            int filled = (int)(pct * barWidth);
            string bar = new string('█', filled) + new string('░', barWidth - filled);
            Console.Write($"\r  [{bar}] {pct * 100:F1}%  {extra}  ");
        }

        public static void Separator() { Console.ForegroundColor = ConsoleColor.DarkGray; Console.WriteLine("  " + new string('─', 62)); Console.ResetColor(); }
        public static void PressAny(string msg = "Davam etmək üçün Enter bas...") { Console.ForegroundColor = ConsoleColor.DarkGray; Console.WriteLine($"\n  {msg}"); Console.ResetColor(); Console.ReadLine(); }
        public static void Stat(string label, string value, ConsoleColor color = ConsoleColor.Cyan) { Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($"  {label,-22}"); Console.ForegroundColor = color; Console.WriteLine(value); Console.ResetColor(); }
    }
}