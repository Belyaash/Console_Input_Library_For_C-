namespace ConsoleInput.Internals.InputRules
{
    internal class ForNumericPlusMinus : IInputRule
    {
        private bool IsNegative { get; set; }

        internal ForNumericPlusMinus()
        {
            IsNegative = false;
        }

        public string TryAddSymbol(string result, char symbol)
        {
            if (symbol == '-')
            {
                if (IsNegative)
                {
                    IsNegative = false;
                    if (result.Length == 1)
                        return "";
                    return result.Substring(1);
                }

                IsNegative = true;
                if (result.Length == 0)
                {
                    return "-";
                }
                return "-" + result;
            };

            if ((symbol == '+') && (IsNegative))
            {
                IsNegative = false;
                return result.Substring(1);
            }

            return result;
        }

        public string RemoveLastSymbol(string result)
        {
            if (result.Length == 0)
                return result;

            if (result[^1] == '-')
            {
                IsNegative = false;
                result = result.Substring(0, result.Length - 1);
            }

            return result;
        }

        public void SetToDefault()
        {
            IsNegative = false;
        }
    }
}