using System.Collections.Generic;
using PassGenPro.Utils;

namespace PassGenPro.Generators
{
    public static class BaseGenerator
    {
        public static readonly string[] BASE_WORDS = {
            "password","123456","password1","abc123","iloveyou","admin","letmein","monkey",
            "qwerty","dragon","master","sunshine","princess","welcome","shadow","superman",
            "football","pass","hello","charlie","batman","trustno1","login","passw0rd",
            "summer","baseball","mustang","access","love","test","1234","12345","000000",
            "666666","654321","123123","121212","112233","azerty","asdfgh","zxcvbn","soccer",
            "hockey","ranger","hunter","harley","justin","chelsea","cheese","thomas",
            "jessica","pepper","killer","daniel","george","andrew","joshua","secret",
            "jordan","computer","internet","flower","tiger","matrix","root","user","guest",
            "demo","backup","temp","info","security","network","server","system","linux",
            "windows","android","iphone","samsung","nokia","huawei","apple","google",
            "facebook","twitter","instagram","youtube","netflix","amazon","microsoft",
            "qwerty123","password123","abc","zxcvbnm","asdfghjkl","1q2w3e","1q2w3e4r",
            "q1w2e3r4","123qwe","asd123","pass123","admin123","root123","test123",
            "111111","222222","333333","444444","555555","777777","888888","999999",
            "123456789","1234567890","baku","azerbaijan","istanbul","ankara","moscow",
            "london","paris","berlin","dubai","tokyo","qafqaz","xazar","neft","kapital",
            "admin1","admin2","root1","toor","alpine","raspberry","openssl","default",
            "changeme","abc1234","pass1234","letmein1","welcome1","monkey1","dragon1",
            "superman1","sunshine1","princess1","master1","shadow1","michael","jennifer",
            "jordan1","batman1","cookie","cheese1","spider","monkey123","dragon123",
            "qazwsx","poiuytrewq","lkjhgfdsa","mnbvcxz","1234qwer","pass@123","P@ssw0rd",
            "Admin@123","Welcome1","Hello123","Test1234","Root@123","User@123"
        };

        public static readonly string[] SYMBOLS = { "!", "@", "#", "$", "_", ".", "*", "?", "&", "%", "-", "+" };
        public static readonly string[] ALL_YEARS;

        static BaseGenerator()
        {
            var years = new List<string>();
            for (int y = 1970; y <= 2025; y++) years.Add(y.ToString());
            ALL_YEARS = years.ToArray();
        }

        public static void AddVariants(HashSet<string> set, string w, Config cfg)
        {
            set.Add(w);
            if (cfg.UseCaseVariants)
            {
                set.Add(w.ToLower());
                set.Add(w.ToUpper());
                set.Add(WordUtils.Capitalize(w));
                if (w.Length > 0) set.Add(w.ToLower() + char.ToUpper(w[0]));
                set.Add(WordUtils.Alternating(w));
            }
            if (cfg.UseLeet)
            {
                var l = WordUtils.Leet(w);
                set.Add(l);
                if (cfg.UseCaseVariants) { set.Add(l.ToUpper()); set.Add(WordUtils.Capitalize(l)); }
            }
            if (cfg.UseReverse)
            {
                var rev = WordUtils.Reverse(w);
                set.Add(rev);
                if (cfg.UseCaseVariants) set.Add(WordUtils.Capitalize(rev));
            }
            if (cfg.UseDuplicate)
            {
                set.Add(w + w);
                set.Add(w.ToUpper() + w.ToLower());
                set.Add(w + w.ToUpper());
            }
        }

        public static void AddWithSuffix(HashSet<string> set, string w, string suffix, Config cfg)
        {
            set.Add(w + suffix);
            set.Add(suffix + w);
            set.Add(w + "_" + suffix);
            set.Add(w + "." + suffix);
            if (cfg.UseCaseVariants)
            {
                set.Add(w.ToUpper() + suffix);
                set.Add(WordUtils.Capitalize(w) + suffix);
                set.Add(suffix + WordUtils.Capitalize(w));
            }
            if (cfg.UseLeet)
            {
                var l = WordUtils.Leet(w);
                set.Add(l + suffix);
                set.Add(suffix + l);
            }
            if (cfg.UseSymbols)
            {
                foreach (var s in SYMBOLS)
                {
                    set.Add(w + suffix + s);
                    set.Add(WordUtils.Capitalize(w) + suffix + s);
                    set.Add(s + w + suffix);
                }
            }
        }
    }
}