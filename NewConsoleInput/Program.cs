
using ConsoleInput;

namespace NewConsoleInput;
using ConsoleInput.Internals;

static class Program
{
    public static void Main()
    {
        int Num = Input.CreateNumber<int>("Input an int", MinMax<int>.LowerThan(0));
    }
}