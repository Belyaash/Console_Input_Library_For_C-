
using System.Globalization;
using ConsoleInput;

namespace NewConsoleInput;
using ConsoleInput.Internals;

static class Program
{
    public static void Main()
    {
        var a = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

        Input.InputOptions().SetCurrencyDecimalSeparator(">>")
            .SetCurrencySymbol("rubles")
            .SetNumberGroupSeparator("  ")
            .SetNumberDecimalSeparator("'")
            .SetCurrencyNegativePattern(0);

        var date = Input.CreateDate("Create Date");
        Console.WriteLine(date);
    }
}