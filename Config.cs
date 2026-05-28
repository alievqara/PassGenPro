using System.Collections.Generic;

namespace PassGenPro
{
    public class Config
    {
        public List<string> Words { get; set; } = new();
        public List<string> Numbers { get; set; } = new();
        public List<string> Names { get; set; } = new();
        public List<string> Surnames { get; set; } = new();
        public List<string> BirthYears { get; set; } = new();
        public List<string> Phones { get; set; } = new();
        public string Essid { get; set; } = "";

        public bool UseLeet { get; set; } = true;
        public bool UseCaseVariants { get; set; } = true;
        public bool UseSymbols { get; set; } = true;
        public bool UseYears { get; set; } = true;
        public bool UseWordCombo { get; set; } = true;
        public bool UseBaseWords { get; set; } = true;
        public bool UseReverse { get; set; } = true;
        public bool UseDuplicate { get; set; } = true;
        public bool UseKeyboardWalks { get; set; } = true;
        public bool UseNamePatterns { get; set; } = true;
        public bool UsePhonePatterns { get; set; } = true;
        public bool UseEssidPatterns { get; set; } = true;
        public bool UseMarkov { get; set; } = true;
        public bool UseDotPatterns { get; set; } = true;
        public bool UseHybrid { get; set; } = true;

        public string OutputFile { get; set; } = "wordlist.txt";
        public int MinLength { get; set; } = 8;
        public int MaxLength { get; set; } = 32;
    }
}