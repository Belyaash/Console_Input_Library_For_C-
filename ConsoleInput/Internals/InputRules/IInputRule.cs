namespace ConsoleInput.Internals.InputRules;

internal interface IInputRule
{
    string TryAddSymbol(string result, char symbol);
    string RemoveLastSymbol(string result);
    void SetToDefault();

}