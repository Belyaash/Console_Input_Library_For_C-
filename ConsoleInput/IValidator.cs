namespace ConsoleInput;

public interface IValidator
{
    string TryAddSymbol(string result, char symbol);
    string ClearString(string result);
    string RemoveLast(string result);
    bool IsValid(string result);
}