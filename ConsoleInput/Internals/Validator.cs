using System.Diagnostics;
using System.Globalization;

namespace ConsoleInput.Internals;

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
}

internal class NumberValidator<T> : IValidator where T : struct, IComparable<T>
{
    private readonly List<IInputRule> inputRules;
    private List<ICheckRule> _checkRules = new();
    private readonly T _min;
    private readonly T _max;
    private NumberValidator(List<IInputRule> rules, T min, T max)
    {
        this._min = min;
        this._max = max;
        this.inputRules = rules;
    }

    public static NumberValidator<T> Create(TypeCode tc, CultureInfo culture, T min, T max)
    {
        List<IInputRule> rules = ValidatorRulesGetter.GetByTypeCode(tc, culture);
        return new NumberValidator<T>(rules,min,max);
    }

    public void AddCheckRules(List<ICheckRule> icr)
    {
        this._checkRules = icr;
    }

    public string TryAddSymbol(string result, char symbol)
    {
        if (_checkRules.Any(rule => !rule.Validate(result)))
        {
            return result;
        }
        string tempResult = result;
        foreach (var rule in inputRules)
        {
            tempResult = rule.TryAddSymbol(result, symbol);
             if (tempResult != result)
                break;
        }

        if (tempResult is "-" or "")
        {
            return tempResult;
        }

        bool isSuccessful = GenericMethods.TryParse<T>(tempResult, out var number);
        return !isSuccessful ? result : tempResult;
    }

    public string ClearString(string result)
    {
        foreach (var rule in inputRules)
        {
            rule.SetToDefault();
        }
        return "";
    }

    public string RemoveLast(string result)
    {
        if (result.Length == 0)
            return result;

        string tempResult = result;
        foreach (var rule in inputRules)
        {
            tempResult = rule.RemoveLastSymbol(result);
            if (tempResult != result)
                break;
        }

        if (tempResult is "-" or "")
        {
            return tempResult;
        }

        bool isSuccessful = GenericMethods.TryParse<T>(tempResult, out var number);
        return !isSuccessful ? result : tempResult;
    }

    public bool IsValid(string result)
    {
        bool isParsed = result.TryParse<T>(out var number);
        return isParsed && _min.CompareTo(number) < 1 && _max.CompareTo(number) > -1;
    }
}