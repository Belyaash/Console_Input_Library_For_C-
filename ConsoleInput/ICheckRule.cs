namespace ConsoleInput;

public interface ICheckRule
{
    bool Validate(string result, char symbol);
}