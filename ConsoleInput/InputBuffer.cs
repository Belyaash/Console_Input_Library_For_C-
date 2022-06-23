namespace ConsoleInput;

public class InputBuffer : IInputBuffer
{
    private IValidator validator;
    public string Result;
    public InputBuffer(IValidator validator)
    {
        this.validator = validator;
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
            default:
            {
                tempResult = validator.TryAddSymbol(Result, cki.KeyChar);
                break;
            }
        }

        if (tempResult == Result)
        {
            Console.Beep();
        }
        else
        {
            Result = tempResult;
        }
    }

    public void PrintCurrentResult()
    {
        ConsoleWriter.OverwriteCurrentLine(Result);
    }
}