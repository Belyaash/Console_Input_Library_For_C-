﻿using ConsoleInput.Internals.InputRules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInput.Internals.Validator;
internal class HexadecimalValidator : IValidator
{
    private readonly List<IInputRule> _inputRules;
    private List<ICheckRule> _checkRules = new();
    private readonly uint _min;
    private readonly uint _max;
    private HexadecimalValidator(List<IInputRule> rules, uint min, uint max)
    {
        _min = min;
        _max = max;
        _inputRules = rules;
    }

    public static HexadecimalValidator Create(CultureInfo culture, uint min, uint max)
    {
        List<IInputRule> rules = ValidatorRulesGetter.GetForHexadecimal();
        return new HexadecimalValidator(rules, min, max);
    }

    public void ReplaceCheckRules(List<ICheckRule> icr)
    {
        _checkRules = icr;
    }

    public string TryAddSymbol(string result, char symbol)
    {
        if (_checkRules.Any(rule => !rule.Validate(result, symbol)))
        {
            return result;
        }
        string tempResult = result;

        foreach (var rule in _inputRules)
        {
            tempResult = rule.TryAddSymbol(result, symbol);
            if (tempResult != result)
                break;
        }

        if (tempResult is "")
        {
            return tempResult;
        }

        return tempResult;
    }

    public string ClearString(string result)
    {
        foreach (var rule in _inputRules)
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
        foreach (var rule in _inputRules)
        {
            tempResult = rule.RemoveLastSymbol(result);
            if (tempResult != result)
                break;
        }

        if (tempResult is "")
        {
            return tempResult;
        }

        return tempResult;
    }

    public bool IsValid(string result)
    {
        bool isParsed = uint.TryParse(result, NumberStyles.HexNumber, null, out var number);
        return isParsed && _min.CompareTo(number) < 1 && _max.CompareTo(number) > -1;
    }
}
