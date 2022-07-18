
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

        int date = Input.CreateNumber<int>("Create int");
        Console.WriteLine(date);


    }
}