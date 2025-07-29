using System;
using System.Globalization;

namespace _Project.Scripts.Utils.NumberFormatting
{
    public sealed class NumberFormattingOptions
    {
        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;
        public bool UseAbsoluteValueForThreshold { get; set; } = true;
        public int MaximumFractionDigits { get; set; } = 2;
        public bool RoundToNearest { get; set; } = false;

        public NumberFormattingOptions Clone()
        {
            return new NumberFormattingOptions
            {
                FormatProvider = this.FormatProvider,
                UseAbsoluteValueForThreshold = this.UseAbsoluteValueForThreshold,
                MaximumFractionDigits = this.MaximumFractionDigits,
                RoundToNearest = this.RoundToNearest
            };
        }
    }
}