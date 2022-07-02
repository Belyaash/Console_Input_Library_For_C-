namespace ConsoleInput.Internals;

public interface IInputBuffer
{
    string Result { get; }
    bool IsValidResult { get; }
    void ProcessInput(ConsoleKeyInfo cki);
    void PrintCurrentResult(string format);
    void PrintResultOnPartOfLine(string format, int leftPos, int rightPos);

}