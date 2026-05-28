using System.Collections.Generic;

namespace PassGenPro.Generators
{
    public static class PhoneGenerator
    {
        public static void Generate(HashSet<string> set, Config cfg)
        {
            foreach (var phone in cfg.Phones)
            {
                // Remove non-digits
                var digits = "";
                foreach (var c in phone) if (char.IsDigit(c)) digits += c;

                if (digits.Length < 7) continue;

                set.Add(digits);
                set.Add(phone); // original format

                // Different prefix formats
                if (digits.Length == 10)
                {
                    set.Add("0" + digits);
                    set.Add("+994" + digits.Substring(1));
                    set.Add("994" + digits.Substring(1));
                    set.Add("+90" + digits);
                    set.Add("+7" + digits);
                    // Parts
                    set.Add(digits.Substring(0, 7));
                    set.Add(digits.Substring(3));
                    set.Add(digits.Substring(digits.Length - 7));
                }

                if (digits.Length >= 9)
                {
                    set.Add(digits.Substring(digits.Length - 9));
                    set.Add(digits.Substring(digits.Length - 8));
                }

                // With symbols
                foreach (var s in new[] { "!", "@", "#" })
                {
                    set.Add(digits + s);
                    set.Add(phone + s);
                }

                // Combine with words
                foreach (var w in cfg.Words)
                {
                    set.Add(w + digits);
                    set.Add(digits + w);
                }
                foreach (var n in cfg.Names)
                {
                    set.Add(n + digits);
                    set.Add(digits + n);
                }
            }
        }
    }
}