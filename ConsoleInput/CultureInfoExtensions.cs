using System.Globalization;

namespace ConsoleInput;

public static class CultureInfoExtensions
{
    public static CultureInfo SetNumberDecimalSeparator(this CultureInfo ci, string separator)
    {
        ci.NumberFormat.NumberDecimalSeparator = separator;
        return ci;
    }

    public static CultureInfo SetNumberGroupSeparator(this CultureInfo ci, string separator)
    {
        ci.NumberFormat.NumberGroupSeparator = separator;
        return ci;
    }

    public static CultureInfo SetNumberGroupSizes(this CultureInfo ci, int[] sizes)
    {
        ci.NumberFormat.NumberGroupSizes = sizes;
        return ci;
    }

    public static CultureInfo SetCurrencyDecimalSeparator(this CultureInfo ci, string separator)
    {
        ci.NumberFormat.CurrencyDecimalSeparator = separator;
        return ci;
    }

    public static CultureInfo SetCurrencyGroupSizes(this CultureInfo ci, int[] sizes)
    {
        ci.NumberFormat.CurrencyGroupSizes = sizes;
        return ci;
    }

    public static CultureInfo SetCurrencySymbol(this CultureInfo ci, string symbol)
    {
        ci.NumberFormat.CurrencySymbol = symbol;
        return ci;
    }

    public static CultureInfo SetCurrencyPositivePattern(this CultureInfo ci, int pattern)
    {
        ci.NumberFormat.CurrencyPositivePattern = pattern;
        return ci;
    }
    public static CultureInfo SetCurrencyNegativePattern(this CultureInfo ci, int pattern)
    {
        ci.NumberFormat.CurrencyNegativePattern = pattern;
        return ci;
    }
}