using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInput.Internals.InputRules.CheckRules;
internal class NativeDigitsCheckRule : ICheckRule
{
    private string[] _validChars;
    internal NativeDigitsCheckRule(string[] nativeDigits)
    {
        _validChars = nativeDigits;
    }

    public bool Validate(string result, char symbol)
    {
        return _validChars.Any(digit => digit[0] == symbol);
    }
}
