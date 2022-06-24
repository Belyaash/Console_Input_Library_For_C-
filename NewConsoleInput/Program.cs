
using ConsoleInput;

namespace NewConsoleInput;
using ConsoleInput.Internals;

static class Program
{
    public static void Main()
    {

        double a = Input.CreateNumber<double>("Enter a int number");
        //var num = Input.CreateNumber<int>("Welcome Message");
        Console.WriteLine(a);
    }
}