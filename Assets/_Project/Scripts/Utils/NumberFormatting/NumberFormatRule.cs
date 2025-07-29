namespace _Project.Scripts.Utils.NumberFormatting
{
    public sealed class NumberFormatRule
    {
        public decimal Threshold { get; }
        public string FormatPattern { get; }
        public string Suffix { get; }

        public NumberFormatRule(decimal threshold, string formatPattern, string suffix)
        {
            Threshold = threshold;
            FormatPattern = formatPattern;
            Suffix = suffix;
        }
    }
}