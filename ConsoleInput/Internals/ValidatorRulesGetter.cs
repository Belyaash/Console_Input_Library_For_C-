using System.Globalization;
using ConsoleInput.Internals.ImputRules;

namespace ConsoleInput.Internals
{
    internal static class ValidatorRulesGetter
    {
        internal static List<IInputRule> GetByTypeCode(TypeCode tc, CultureInfo ci)
        {
            switch (tc)
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return NonDecimalNegativeRules(ci);

                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return NonDecimalOnlyPositiveRules(ci);

                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.Decimal:
                    return DecimalRules(ci);

                default:
                    throw new ArgumentOutOfRangeException(nameof(tc), tc, null);
            }
        }

        private static List<IInputRule> NonDecimalOnlyPositiveRules(CultureInfo ci)
        {
            var rules = new List<IInputRule>();
            rules.Add(new ForNumeric(ci));
            return rules;
        }

        private static List<IInputRule> NonDecimalNegativeRules(CultureInfo ci)
        {
            var rules = NonDecimalOnlyPositiveRules(ci);
            rules.Add(new ForNumericPlusMinus());
            return rules;
        }

        private static List<IInputRule> DecimalRules(CultureInfo ci)
        {
            var rules = NonDecimalNegativeRules(ci);
            rules.Add(new ForDecimalSeparator(ci));
            return rules;
        }
    }
}