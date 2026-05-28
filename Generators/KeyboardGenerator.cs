using System.Collections.Generic;
using PassGenPro.Utils;

namespace PassGenPro.Generators
{
    public static class KeyboardGenerator
    {
        static readonly string[] WALKS = {
            "qwerty","qwertyuiop","asdfgh","asdfghjkl","zxcvbn","zxcvbnm",
            "1qaz2wsx","2wsx3edc","qazwsx","qazwsxedc","1q2w3e4r","1q2w3e4r5t",
            "qweqwe","asdasd","zxczxc","qweasdzxc","!qaz@wsx","1qaz!qaz",
            "qwerty1","qwerty12","qwerty123","qwerty1234","asdf1234","zxcv1234",
            "1234qwer","4321rewq","poiuytrewq","mnbvcxz","lkjhgfdsa",
            "123qweasd","qweasd123","asdqwe123","1234asdf","asdf4321",
            "qaywsx","qscwdv","qazxsw","1qazxsw2","!QAZ@WSX",
            "qwerty!","qwerty@","qwerty#","Qwerty1","Qwerty12","Qwerty123",
            "QWERTY","ASDFGH","ZXCVBN","QWERTYUIOP","ASDFGHJKL",
        };

        public static void Generate(HashSet<string> set, Config cfg)
        {
            foreach (var w in WALKS)
            {
                set.Add(w);
                if (cfg.UseCaseVariants)
                {
                    set.Add(w.ToUpper());
                    set.Add(WordUtils.Capitalize(w));
                }
                if (cfg.UseLeet) set.Add(WordUtils.Leet(w));
                if (cfg.UseSymbols)
                    foreach (var s in new[] { "!", "@", "1", "123" })
                    {
                        set.Add(w + s);
                        set.Add(WordUtils.Capitalize(w) + s);
                    }
                // Combine with user words
                foreach (var uw in cfg.Words)
                {
                    set.Add(w + uw);
                    set.Add(uw + w);
                }
            }
        }
    }
}