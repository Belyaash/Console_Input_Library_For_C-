
using ConsoleInput;

namespace NewConsoleInput;
using ConsoleInput.Internals;

static class Program
{
    public static void Main()
    {
        double Num = Input.CreateNumber<double>("Input an int", MinMax<double>.LowerThan(0.0001));
    }
}