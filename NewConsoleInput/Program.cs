
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

        string[] _validChars = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
        //ConsoleKeyInfo cki = Console.ReadKey();
        //bool isValid = _validChars.Contains(cki.KeyChar.ToString().ToUpper());
        //Console.WriteLine(isValid);

        uint hex = Input.InputHexadecimal("Print a hexadecimal",MinMax<uint>.HigherThan(0));
        Console.WriteLine();
        Console.WriteLine(hex);

    }
}