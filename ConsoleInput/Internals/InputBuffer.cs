using System.Globalization;

namespace ConsoleInput.Internals;

internal class InputBuffer : IInputBuffer
{
    private IValidator validator;
    private CultureInfo culture;
    private TypeCode typeCode;
    public string Result { get; private set; }
    public bool IsValidResult { get; private set; }
    public InputBuffer(IValidator validator, CultureInfo ci, TypeCode typeCode)
    {
        this.validator = validator;
        this.culture = ci;
        this.Result = "";
        this.typeCode = typeCode;
    }
    public void ProcessInput(ConsoleKeyInfo cki)
    {
        bool isEnter = false;
        string tempResult = Result;
        switch (cki.Key)
        {
            case ConsoleKey.Backspace:
            {
                tempResult = validator.RemoveLast(Result);
                break;
            }
            case ConsoleKey.Escape:
            {
                tempResult = validator.ClearString(Result);
                break;
            }
            case ConsoleKey.Enter:
            {
                isEnter = true;
                IsValidResult = validator.IsValid(Result);
                if (!IsValidResult)
                    Console.Beep();
                break;
            }
            default:
            {
                tempResult = validator.TryAddSymbol(Result, cki.KeyChar);
                break;
            }
        }
        if (isEnter)
            return;

        if (tempResult == Result)
            Console.Beep();
        else
            Result = tempResult;
    }

    public void PrintCurrentResult()
    {
        string format = "#,#.###;-#,#.###;0";

        ConsoleWriter.OverwriteCurrentLine(StringFormatter.TypeCodeToString(typeCode, format, Result,
            culture));
    }
}