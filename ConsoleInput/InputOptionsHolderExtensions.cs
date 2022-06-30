using System.Globalization;

namespace ConsoleInput;

public static class InputOptionsHolderExtensions
{
    public static InputOptionsHolder SetNumberDecimalSeparator(this InputOptionsHolder optionsHolder, string separator)
    {
        optionsHolder.NumberDecimalSeparator = separator;
        return optionsHolder;
    }

    public static InputOptionsHolder SetNumberGroupSeparator(this InputOptionsHolder optionsHolder, string separator)
    {
        optionsHolder.NumberGroupSeparator = separator;
        return optionsHolder;
    }

    public static InputOptionsHolder SetNumberGroupSizes(this InputOptionsHolder optionsHolder, int[] sizes)
    {
        optionsHolder.NumberGroupSizes = sizes;
        return optionsHolder;
    }

    public static InputOptionsHolder SetCurrencyDecimalSeparator(this InputOptionsHolder optionsHolder, string separator)
    {
        optionsHolder.CurrencyDecimalSeparator = separator;
        return optionsHolder;
    }

    public static InputOptionsHolder SetCurrencyGroupSizes(this InputOptionsHolder optionsHolder, int[] sizes)
    {
        optionsHolder.CurrencyGroupSizes = sizes;
        return optionsHolder;
    }

    public static InputOptionsHolder SetCurrencySymbol(this InputOptionsHolder optionsHolder, string symbol)
    {
        optionsHolder.CurrencySymbol = symbol;
        return optionsHolder;
    }

    public static InputOptionsHolder SetCurrencyPositivePattern(this InputOptionsHolder optionsHolder, int pattern)
    {
        optionsHolder.CurrencyPositivePattern = pattern;
        return optionsHolder;
    }
    public static InputOptionsHolder SetCurrencyNegativePattern(this InputOptionsHolder optionsHolder, int pattern)
    {
        optionsHolder.CurrencyNegativePattern = pattern;
        return optionsHolder;
    }
}