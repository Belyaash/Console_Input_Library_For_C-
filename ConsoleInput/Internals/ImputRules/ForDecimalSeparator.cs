using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInput.Internals.ImputRules;
internal class ForDecimalSeparator : IInputRule
{
    private readonly string separator;
    private bool haveSep { set; get; }

    internal ForDecimalSeparator(CultureInfo ci)
    {
        this.separator = ci.NumberFormat.NumberDecimalSeparator;
        haveSep = false;
    }

    public string ChangingString(string value, char input)
    {
        if (input == separator[0])
        {
            return value + separator;
        }
        return value;
    }

    public string TryAddSymbol(string result, char symbol)
    {
        if ((!haveSep) && (symbol == separator[0]))
        {
            haveSep = true;
            result += separator;
        }
        return result;
    }

    public string RemoveLastSymbol(string result)
    {
        if (result[^1] == separator[^1])
        {
            try
            {
                result = result.Substring(0, result.Length - separator.Length);
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