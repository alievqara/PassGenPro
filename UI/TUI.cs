using System;
using System.Collections.Generic;

namespace PassGenPro.UI
{
    public static class TUI
    {
        const int WIDTH = 60;
        static string Line(char c = '-') => new string(c, WIDTH);

        public static void Clear() => Console.Clear();

        public static void Banner()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  +{Line()}+");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  |  Dobby Password Generator{new string(' ', WIDTH - 27)}|");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  |  v1.3  |  by Dobby{new string(' ', WIDTH - 22)}|");
            Console.WriteLine($"  +{Line()}+");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void SectionHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  +{Line()}+");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  |  {title}{new string(' ', Math.Max(0, WIDTH - title.Length - 3))}|");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  +{Line()}+");
            Console.ResetColor();
            Console.WriteLine();
        }

        public static int ArrowMenu(string[] options, string? title = null)
        {
            int selected = 0;
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Banner();

                if (title != null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"  +{Line()}+");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"  |  {title}{new string(' ', Math.Max(0, WIDTH - title.Length - 3))}|");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"  +{Line()}+");
                    Console.ResetColor();
                }

                Console.WriteLine();
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selected)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  | ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"  {i + 1}. ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(options[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  | ");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"  {i + 1}. {options[i]}");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  +{Line()}+");
                Console.ResetColor();

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow) selected = (selected - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.DownArrow) selected = (selected + 1) % options.Length;
                else if (key == ConsoleKey.Enter) break;
                else if (key >= ConsoleKey.D1 && key <= ConsoleKey.D9)
                {
                    int idx = key - ConsoleKey.D1;
                    if (idx < options.Length) { selected = idx; break; }
                }
            }

            Console.CursorVisible = true;
            return selected;
        }

        public static bool[] CheckboxMenu(string[] options, bool[]? defaults = null)
        {
            bool[] selected = new bool[options.Length];
            if (defaults != null)
                for (int i = 0; i < Math.Min(defaults.Length, selected.Length); i++)
                    selected[i] = defaults[i];

            int cursor = 0;
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Banner();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  +{Line()}+");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  |  SPACE seç   A=hamısı   N=heç biri   ENTER bitir{new string(' ', WIDTH - 51)}|");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  +{Line()}+");
                Console.ResetColor();
                Console.WriteLine();

                for (int i = 0; i < options.Length; i++)
                {
                    string check = selected[i] ? "◉" : "○";
                    if (i == cursor)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  | ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($"  {check} ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(options[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"  |  ");
                        Console.ForegroundColor = selected[i] ? ConsoleColor.DarkGray : ConsoleColor.DarkGray;
                        Console.Write($" {check} ");
                        Console.ForegroundColor = selected[i] ? ConsoleColor.White : ConsoleColor.DarkGray;
                        Console.WriteLine(options[i]);
                        Console.ResetColor();
                    }
                }

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  +{Line()}+");
                Console.ResetColor();

                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow) cursor = (cursor - 1 + options.Length) % options.Length;
                else if (key == ConsoleKey.DownArrow) cursor = (cursor + 1) % options.Length;
                else if (key == ConsoleKey.Spacebar) selected[cursor] = !selected[cursor];
                else if (key == ConsoleKey.Enter) break;
                else if (key == ConsoleKey.A) for (int i = 0; i < selected.Length; i++) selected[i] = true;
                else if (key == ConsoleKey.N) for (int i = 0; i < selected.Length; i++) selected[i] = false;
            }

            Console.CursorVisible = true;
            Console.WriteLine();
            return selected;
        }

        public static string Prompt(string label, string def = "")
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  | ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"  {label}");
            if (!string.IsNullOrEmpty(def)) { Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($" [{def}]"); }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(": ");
            Console.ResetColor();
            var r = Console.ReadLine()?.Trim() ?? "";
            return string.IsNullOrEmpty(r) ? def : r;
        }

        public static List<string> PromptList(string label, string hint = "")
        {
            var list = new List<string>();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  | ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  {label}");
            if (!string.IsNullOrEmpty(hint))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"  |    ({hint})");
            }
            Console.ResetColor();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"  |    [{list.Count + 1}] ");
                Console.ForegroundColor = ConsoleColor.White;
                var v = Console.ReadLine()?.Trim() ?? "";
                Console.ResetColor();
                if (string.IsNullOrEmpty(v)) break;
                list.Add(v);
            }
            return list;
        }

        public static void ProgressBar(long current, long total, string extra = "", int barWidth = 40)
        {
            double pct = total == 0 ? 1 : (double)current / total;
            int filled = (int)(pct * barWidth);
            string bar = new string('█', filled) + new string('░', barWidth - filled);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("\r  | ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"  [{bar}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{pct * 100:F1}%  {extra}  ");
            Console.ResetColor();
        }

        public static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  | ");
            Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"  ✓  {msg}");
            Console.ResetColor();
        }

        public static void Warn(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  | ");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"  ⚡ {msg}");
            Console.ResetColor();
        }

        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  | ");
            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"  ✗  {msg}");
            Console.ResetColor();
        }

        public static void Dim(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  |    {msg}");
            Console.ResetColor();
        }

        public static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  | ");
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine($"  {msg}");
            Console.ResetColor();
        }

        public static void Stat(string label, string value, ConsoleColor color = ConsoleColor.Cyan)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("  | ");
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write($"  {label,-20}");
            Console.ForegroundColor = color; Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void Separator()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  +{Line()}+");
            Console.ResetColor();
        }

        public static void PressAny(string msg = "Davam etmək üçün Enter bas...")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  +{Line()}+");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  |  {msg}{new string(' ', Math.Max(0, WIDTH - msg.Length - 3))}|");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  +{Line()}+");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}