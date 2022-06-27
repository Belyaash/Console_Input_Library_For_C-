namespace ConsoleInput.Internals;

internal interface IInputRule
{
    string TryAddSymbol(string result, char symbol);
    string RemoveLastSymbol(string result);
    void SetToDefault();

}