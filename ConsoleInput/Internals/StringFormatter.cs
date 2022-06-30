using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInput.Internals
{
    internal class StringFormatter 
    {
        //private static string NonDecimalToString<T>(string format, string result, CultureInfo ci) where T : struct
        //{

        //    bool isParsed = result.TryParse<T>(out var number);
        //    return isParsed ? number.ToString(format, ci) : result;
        //}
        internal static string TypeCodeToString(TypeCode typeCode, string format, string result, CultureInfo ci)
        {
            switch (typeCode)
            {
                case TypeCode.SByte:
                    try
                    {
                        var number = sbyte.Parse(result);
                        return number.ToString(format, ci);
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.Byte:
                    try
                    {
                        var number = byte.Parse(result);
                        return number.ToString(format, ci);
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.Int16:
                    try
                    {
                        var number = short.Parse(result);
                        return number.ToString(format, ci);
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.UInt16:
                    try
                    {
                        var number = ushort.Parse(result);
                        return number.ToString(format, ci);
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.Int32:
                    try
                    {
                        var number = int.Parse(result);
                        return number.ToString(format, ci);
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.UInt32:
                    try
                    {
                        var number = uint.Parse(result);
                        return number.ToString(format, ci);
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.Int64:
                    try
                    {
                        var number = long.Parse(result);
                        return number.ToString(format, ci);
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.UInt64:
                    try
                    {
                        var number = ulong.Parse(result);
                        return number.ToString(format, ci);
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.Single:
                    try
                    {
                        var number = float.Parse(result);
                        string toPrint = number.ToString(format, ci);
                        if (ci.Parent.NumberFormat.NumberDecimalSeparator[^1] == result[^1])
                            toPrint += ci.NumberFormat.NumberDecimalSeparator;
                        return toPrint;
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.Double:
                    try
                    {
                        var number = double.Parse(result);
                        string toPrint = number.ToString(format, ci);
                        if (ci.Parent.NumberFormat.NumberDecimalSeparator[^1] == result[^1])
                            toPrint += ci.NumberFormat.NumberDecimalSeparator;
                        return toPrint;
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                case TypeCode.Decimal:
                    try
                    {
                        var number = decimal.Parse(result);
                        string toPrint = number.ToString(format, ci);
                        if (ci.Parent.NumberFormat.NumberDecimalSeparator[^1] == result[^1])
                            toPrint += ci.NumberFormat.NumberDecimalSeparator;
                        return toPrint;
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                default:
                    return result;
            }
        }
    }
}