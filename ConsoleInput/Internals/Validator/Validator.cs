using System.Diagnostics;
using System.Globalization;

namespace ConsoleInput.Internals.Validator;

internal class Validator
{
    public static IValidator GetByTypeCode<T>(TypeCode tc, CultureInfo culture, T min, T max) where T : struct, IComparable<T>
    {
        switch (tc)
        {
            case TypeCode.Int32:
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
                return NumberValidator<T>.Create(tc, culture, min, max);
            default:
                throw new ArgumentOutOfRangeException(nameof(tc), tc, null);
        }
    }

    public static IValidator GetForHexadecimal(CultureInfo culture, uint min, uint max)
    {
        return HexadecimalValidator.Create(culture, min, max);
    }
}