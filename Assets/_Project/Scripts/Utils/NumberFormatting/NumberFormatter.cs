using System;
using System.Collections.Generic;

namespace _Project.Scripts.Utils.NumberFormatting
{
    /// <summary>
    /// Provides advanced number formatting capabilities with configurable rules and options
    /// </summary>
    public static class NumberFormatter
    {
        private static readonly Dictionary<string, NumberFormatRule> DefaultFormatRules;
        private static readonly object _lock = new object();

        static NumberFormatter()
        {
            DefaultFormatRules = new Dictionary<string, NumberFormatRule>
            {
                ["Billion"] = new NumberFormatRule(1_000_000_000m, "0,,,.####", "B"),
                ["Million"] = new NumberFormatRule(1_000_000m, "0,,.###", "M"),
                ["Thousand"] = new NumberFormatRule(1_000m, "0,.##", "K"),
                ["Default"] = new NumberFormatRule(0m, "0.##", string.Empty)
            };
        }

        private static readonly NumberFormattingOptions DefaultOptions = new NumberFormattingOptions();

        /// <summary>
        /// Formats a number using the default formatting rules and options
        /// </summary>
        public static string FormatNumber(this float number) =>
            FormatNumber(number, DefaultFormatRules, DefaultOptions);

        public static string FormatNumber(this int number) =>
            FormatNumber((float)number, DefaultFormatRules, DefaultOptions);

        /// <summary>
        /// Formats a number using custom formatting rules and options
        /// </summary>
        /// <param name="number">The number to format</param>
        /// <param name="rules">Custom formatting rules to apply</param>
        /// <param name="options">Formatting options</param>
        /// <returns>The formatted string representation</returns>
        public static string FormatNumber<T>(
            this T number,
            IDictionary<string, NumberFormatRule> rules,
            NumberFormattingOptions options) where T : struct, IConvertible
        {
            try
            {
                float floatValue = Convert.ToSingle(number);
                var absValue = options.UseAbsoluteValueForThreshold ? Math.Abs(floatValue) : floatValue;
                var rule = DetermineFormatRule(absValue, rules);

                return FormatNumberWithRule(floatValue, rule, options);
            }
            catch (Exception ex)
            {
                return Convert.ToSingle(number).ToString(options.FormatProvider);
            }
        }

        /// <summary>
        /// Creates a new formatter with custom rules while preserving existing rules
        /// </summary>
        /// <param name="customRules">The custom rules to add or update</param>
        /// <returns>A new dictionary containing merged rules</returns>
        public static Dictionary<string, NumberFormatRule> CreateCustomRules(
            params (string name, decimal threshold, string pattern, string suffix)[] customRules)
        {
            var rules = new Dictionary<string, NumberFormatRule>(DefaultFormatRules);

            foreach (var (name, threshold, pattern, suffix) in customRules)
            {
                rules[name] = new NumberFormatRule(threshold, pattern, suffix);
            }

            return rules;
        }

        private static NumberFormatRule DetermineFormatRule(
            float value,
            IDictionary<string, NumberFormatRule> rules)
        {
            var absValue = Math.Abs(value);
            NumberFormatRule selectedRule = rules["Default"];
            decimal highestThreshold = decimal.MinValue;

            foreach (var rule in rules.Values)
            {
                if (absValue >= (float)rule.Threshold && rule.Threshold > highestThreshold)
                {
                    selectedRule = rule;
                    highestThreshold = rule.Threshold;
                }
            }

            return selectedRule;
        }

        private static string FormatNumberWithRule(
            float number,
            NumberFormatRule rule,
            NumberFormattingOptions options)
        {
            if (options.RoundToNearest)
            {
                number = RoundToSignificantDigits(number, options.MaximumFractionDigits);
            }

            return $"{number.ToString(rule.FormatPattern, options.FormatProvider)}{rule.Suffix}";
        }

        private static float RoundToSignificantDigits(float number, int digits)
        {
            if (Math.Abs(number) < float.Epsilon) return 0;

            var scale = (float)Math.Pow(10, digits - (int)Math.Floor(Math.Log10(Math.Abs(number))) - 1);
            return (float)Math.Round(number * scale) / scale;
        }
    }
}