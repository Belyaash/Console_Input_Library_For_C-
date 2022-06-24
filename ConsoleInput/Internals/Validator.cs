using System.Diagnostics;
using System.Globalization;

namespace ConsoleInput.Internals;

internal class Validator
{
    public static IValidator GetByTypeCode<T>(TypeCode tc, CultureInfo culture)
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
            case TypeCode.DateTime:
                return NumberValidator<T>.Create(tc, culture);
            default:
                throw new ArgumentOutOfRangeException(nameof(tc), tc, null);
        }
    }
}

internal class NumberValidator<T> : IValidator
{
    private List<IInputRule> inputRules;
    private NumberValidator(List<IInputRule> rules)
    {
        this.inputRules = rules;
    }

    public static NumberValidator<T> Create(TypeCode tc, CultureInfo culture)
    {
        List<IInputRule> rules = ValidatorRulesGetter.GetByTypeCode(tc, culture);
        return new NumberValidator<T>(rules);
    }

    public string TryAddSymbol(string result, char symbol)
    {
        string tempResult = result;
        foreach (var rule in inputRules)
        {
            tempResult = rule.TryAddSymbol(result, symbol);
            Debug.Write(tempResult);
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
        return result.TryParse<T>(out var number);
    }
}

internal interface IInputRule
{
    string TryAddSymbol(string result, char symbol);
    string RemoveLastSymbol(string result);
    void SetToDefault();

}