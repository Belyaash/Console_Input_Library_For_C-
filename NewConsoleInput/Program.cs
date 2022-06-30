
using ConsoleInput;

namespace NewConsoleInput;
using ConsoleInput.Internals;

public class DateCheckRule : ICheckRule
{
    public bool Validate(string result)
    {
        return result.Length < 2;
    }
}
static class Program
{
    public static void Main()
    {
        List<ICheckRule> icr = new List<ICheckRule>();
        icr.Add(new DateCheckRule());

        Input.InputOptions().SetCurrencyDecimalSeparator(">>")
            .SetCurrencySymbol("rubles")
            .SetNumberGroupSeparator("  ")
            .SetNumberDecimalSeparator("'")
            .SetCurrencyNegativePattern(0);
            

        double Num = Input.CreateNumber<double>("Input an int", MinMax<double>.LowerThan(31));
    }
}