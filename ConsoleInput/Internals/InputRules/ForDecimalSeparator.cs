using System.Globalization;

namespace ConsoleInput.Internals.InputRules;
internal class ForDecimalSeparator : IInputRule
{
    private readonly string _separator;
    private bool haveSep { set; get; }

    internal ForDecimalSeparator(CultureInfo ci)
    {
        this._separator = ci.Parent.NumberFormat.NumberDecimalSeparator;
        haveSep = false;
    }

    public string ChangingString(string value, char input)
    {
        if (input == _separator[0])
        {
            return value + _separator;
        }
        return value;
    }

    public string TryAddSymbol(string result, char symbol)
    {
        if ((!haveSep) && (symbol == _separator[0]))
        {
            haveSep = true;
            result += _separator;
        }
        return result;
    }

    public string RemoveLastSymbol(string result)
    {
        if (result[^1] == _separator[^1])
        {
            try
            {
                result = result.Substring(0, result.Length - _separator.Length);
                haveSep = false;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        return result;
    }

    public void SetToDefault()
    {
        haveSep = false;
    }
}