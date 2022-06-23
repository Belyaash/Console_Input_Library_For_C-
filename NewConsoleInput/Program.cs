namespace NewConsoleInput;
using ConsoleInput;

static class Programm
{
    public static void Main()
    {
        var num = Input.CreateNumber<int>("Welcome Message");
        Console.WriteLine(num);

    }
}