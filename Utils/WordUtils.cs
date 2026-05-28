using System.Text;

namespace PassGenPro.Utils
{
    public static class WordUtils
    {
        public static string Leet(string w)
        {
            return w.Replace("a", "@").Replace("A", "@")
                    .Replace("e", "3").Replace("E", "3")
                    .Replace("i", "1").Replace("I", "1")
                    .Replace("o", "0").Replace("O", "0")
                    .Replace("s", "$").Replace("S", "$")
                    .Replace("t", "7").Replace("T", "7")
                    .Replace("g", "9").Replace("G", "9")
                    .Replace("b", "8").Replace("B", "8")
                    .Replace("l", "1").Replace("L", "1");
        }

        public static string Capitalize(string w)
        {
            if (string.IsNullOrEmpty(w)) return w;
            return char.ToUpper(w[0]) + w.Substring(1).ToLower();
        }

        public static string Reverse(string w)
        {
            var arr = w.ToCharArray();
            System.Array.Reverse(arr);
            return new string(arr);
        }

        public static string Alternating(string w)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < w.Length; i++)
                sb.Append(i % 2 == 0 ? char.ToUpper(w[i]) : char.ToLower(w[i]));
            return sb.ToString();
        }

        public static string FormatSize(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
            if (bytes < 1024L * 1024 * 1024) return $"{bytes / (1024.0 * 1024):F1} MB";
            return $"{bytes / (1024.0 * 1024 * 1024):F2} GB";
        }
    }
}