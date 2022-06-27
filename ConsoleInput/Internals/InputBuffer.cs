using System.Globalization;

namespace ConsoleInput.Internals;

internal class InputBuffer : IInputBuffer
{
    private IValidator validator;
    private CultureInfo culture;
    private TypeCode typeCode;
    private List<ConsoleKeyInfo> keys = new List<ConsoleKeyInfo>();
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
        string tempResult;
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
                IsValidResult = validator.IsValid(Result);
                if (!IsValidResult)
                    Console.Beep();
                return;
            }
            case ConsoleKey.Delete:
            {
                if (keys.Count == 0)
                    return;

                var currentKeys = keys.GetRange(0,keys.Count-1);
                keys.Clear();
                Result = validator.ClearString(Result);
                foreach (var key in currentKeys.ToList())
                {
                    ProcessInput(key);
                }

                return;
            }
            default:
            {
                tempResult = validator.TryAddSymbol(Result, cki.KeyChar);
                break;
            }
        }

        if (tempResult == Result)
            Console.Beep();
        else
        {
            Result = tempResult;
            keys.Add(cki);
        }
    }

    public void PrintCurrentResult()
    {
        string format = "#,#.###;-#,#.###;0";

        ConsoleWriter.OverwriteCurrentLine(StringFormatter.TypeCodeToString(typeCode, format, Result,
            culture));
    }
}