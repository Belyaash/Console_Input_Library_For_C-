using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInput.Internals.InputRules;

internal class ForHexadecimal : IInputRule
{
    private readonly string[] _validChars = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

    internal ForHexadecimal()
    {
    }

    public string TryAddSymbol(string result, char symbol)
    {
        bool isValid = _validChars.Contains(symbol.ToString().ToUpper());
        if (!isValid)
            return result;

        if ((result.Length == 1) && (result[0] == _validChars[0][0]))
            return symbol.ToString().ToUpper();

        return result + symbol.ToString().ToUpper();
    }

    public string RemoveLastSymbol(string result)
    {
        return result.Length <= 1 ? "" : result.Substring(0, result.Length - 1);
    }

    public void SetToDefault()
    {
        return;
    }
}
