using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInput.Internals.InputRules.CheckRules;
internal class LengthCheckRule : ICheckRule
{
    private readonly int _maxLength;
    internal LengthCheckRule(int maxLength)
    {
        this._maxLength = maxLength;
    }
    public bool Validate(string result , char symbol)
    {
        return result.Length < _maxLength;
    }
}
