using System.Globalization;

namespace ConsoleInput.Internals.InputRules
{
    internal class ForNumeric : IInputRule
    {
        private readonly string[] validChars;

        internal ForNumeric(CultureInfo ci)
        {
            validChars = ci.NumberFormat.NativeDigits;
        }

        public string TryAddSymbol(string result, char symbol)
        {
            bool isNumber = false;
            foreach (var number in validChars)
            {
                if (number[0] == symbol)
                {
                    isNumber = true;
                    break;
                }
            }

            if (!isNumber) 
                return result;

            if ((result.Length == 1) && (result[0] == validChars[0][0]))
                return symbol.ToString();

            if ((result.Length == 2) && (result[0] == '-') && (result[1] == validChars[0][0]))
                return "-" + symbol.ToString();

            return result + symbol;
        }

        public string RemoveLastSymbol(string result)
        {
            bool isNumber = false;
            foreach (var number in validChars)
            {
                if (number[0] == result[^1])
                {
                    isNumber = true;
                    break;
                }
            }

            if (!isNumber)
                return result;

            if (result.Length == 1)
            {
                return "";
            }
            return result.Substring(0, result.Length - 1);
        }

        public void SetToDefault()
        {
            return;
        }
    }
}
