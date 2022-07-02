using System.Globalization;

namespace ConsoleInput.Internals;

internal class InputBuffer : IInputBuffer
{
    private readonly IValidator _validator;
    private readonly CultureInfo _culture;
    private readonly TypeCode _typeCode;
    private List<ConsoleKeyInfo> keys = new();
    public string Result { get; private set; }
    public bool IsValidResult { get; private set; }
    public InputBuffer(IValidator validator, CultureInfo ci, TypeCode typeCode)
    {
        this._validator = validator;
        this._culture = ci;
        this.Result = "";
        this._typeCode = typeCode;
    }
    public void ProcessInput(ConsoleKeyInfo cki)
    {
        string tempResult;
        switch (cki.Key)
        {
            case ConsoleKey.Backspace:
            {
                tempResult = _validator.RemoveLast(Result);
                break;
            }
            case ConsoleKey.Escape:
            {
                tempResult = _validator.ClearString(Result);
                break;
            }
            case ConsoleKey.Enter:
            {
                IsValidResult = _validator.IsValid(Result);
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
                Result = _validator.ClearString(Result);
                foreach (var key in currentKeys.ToList())
                {
                    ProcessInput(key);
                }

                return;
            }
            default:
            {
                tempResult = _validator.TryAddSymbol(Result, cki.KeyChar);
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

    public void PrintCurrentResult(string format)
    {
        ConsoleWriter.OverwriteCurrentLine(StringFormatter.TypeCodeToString(_typeCode, format, Result,
            _culture));
    }

    public void PrintResultOnPartOfLine(string format, int leftPos, int rightPos)
    {
        ConsoleWriter.OverwritePartOfCurrentLine(StringFormatter.TypeCodeToString(_typeCode, format, Result,
            _culture), leftPos, rightPos);
    }

}