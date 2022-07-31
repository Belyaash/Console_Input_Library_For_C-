using ConsoleInput.Internals.InputRules;
using System.Globalization;

namespace ConsoleInput.Internals.Validator;

internal class NumberValidator<T> : IValidator where T : struct, IComparable<T>
{
    private readonly List<IInputRule> _inputRules;
    private List<ICheckRule> _checkRules = new();
    private readonly T _min;
    private readonly T _max;

    private NumberValidator(List<IInputRule> rules, T min, T max)
    {
        _min = min;
        _max = max;
        _inputRules = rules;
    }

    public static NumberValidator<T> Create(TypeCode tc, CultureInfo culture, T min, T max)
    {
        List<IInputRule> rules = ValidatorRulesGetter.GetByTypeCode(tc, culture);
        return new NumberValidator<T>(rules, min, max);
    }

    public void ReplaceCheckRules(List<ICheckRule> icr)
    {
        _checkRules = icr;
    }

    public string TryAddSymbol(string result, char symbol)
    {
        if (_checkRules.Any(rule => !rule.Validate(result, symbol)))
            return result;

        string tempResult = TryAddSymbolForAllRules(result, symbol);
        return IsTempResultValid(tempResult) ? tempResult : result;
    }

    private string TryAddSymbolForAllRules(string result, char symbol)
    {
        string tempResult = result;
        foreach (var rule in _inputRules)
        {
            tempResult = rule.TryAddSymbol(result, symbol);
            if (tempResult != result)
                break;
        }

        return tempResult;
    }

    public string RemoveLast(string result)
    {
        var tempResult = TryRemoveSymbolForAllRules(result);

        return IsTempResultValid(tempResult) ? tempResult : result;
    }

    private string TryRemoveSymbolForAllRules(string result)
    {
        string tempResult = result;
        foreach (var rule in _inputRules)
        {
            tempResult = rule.RemoveLastSymbol(result);
            if (tempResult != result)
                break;
        }

        return tempResult;
    }

    private static bool IsTempResultValid(string tempResult)
    {
        if (tempResult is "-" or "")
        {
            return true;
        }

        return tempResult.TryParse<T>(out var number);
    }

    public string ClearString(string result)
    {
        SetRulesToDefault();
        return "";
    }

    private void SetRulesToDefault()
    {
        foreach (var rule in _inputRules)
        {
            rule.SetToDefault();
        }
    }

    public bool IsValid(string result)
    {
        bool isParsed = result.TryParse<T>(out var number);
        return isParsed && _min.CompareTo(number) < 1 && _max.CompareTo(number) > -1;
    }
}