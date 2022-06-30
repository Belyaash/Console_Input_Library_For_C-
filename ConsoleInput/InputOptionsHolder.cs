using System.Globalization;

namespace ConsoleInput;
public class InputOptionsHolder
{
    internal InputOptionsHolder(CultureInfo ci)
    {
        _numberDecimalSeparator = ci.NumberFormat.NumberDecimalSeparator;
        _numberGroupSeparator = ci.NumberFormat.NumberGroupSeparator;
        _numberGroupSizes = ci.NumberFormat.NumberGroupSizes;
        _currencyDecimalSeparator = ci.NumberFormat.CurrencyDecimalSeparator;
        _currencyGroupSizes = ci.NumberFormat.CurrencyGroupSizes;
        _currencySymbol = ci.NumberFormat.CurrencySymbol;
        _currencyPositivePattern = ci.NumberFormat.CurrencyPositivePattern;
        _currencyNegativePattern = ci.NumberFormat.CurrencyNegativePattern;
    }

    private string _numberDecimalSeparator;
    private string _numberGroupSeparator;
    private int[] _numberGroupSizes;
    private string _currencyDecimalSeparator;
    private int[] _currencyGroupSizes;
    private string _currencySymbol;
    private int _currencyPositivePattern;
    private int _currencyNegativePattern;

    internal string NumberDecimalSeparator
    {
        get => _numberDecimalSeparator;
        set
        {
            _numberDecimalSeparator = value;
            Input.CultureInfo.NumberFormat.NumberDecimalSeparator = value;
        }
    }

    internal string NumberGroupSeparator
    {
        get => _numberGroupSeparator;
        set
        {
            _numberGroupSeparator = value;
            Input.CultureInfo.NumberFormat.NumberGroupSeparator = value;
        }
    }

    internal int[] NumberGroupSizes
    {
        get => _numberGroupSizes;
        set
        {
            _numberGroupSizes = value;
            Input.CultureInfo.NumberFormat.NumberGroupSizes = _numberGroupSizes;
        }
    }

    internal string CurrencyDecimalSeparator
    {
        get => _currencyDecimalSeparator;
        set
        {
            _currencyDecimalSeparator = value;
            Input.CultureInfo.NumberFormat.CurrencyDecimalSeparator = value;
        }
    }

    internal int[] CurrencyGroupSizes
    {
        get => _currencyGroupSizes;
        set
        {
            _currencyGroupSizes = value;
            Input.CultureInfo.NumberFormat.CurrencyGroupSizes = _currencyGroupSizes;
        }
    }

    internal string CurrencySymbol
    {
        get => _currencySymbol;
        set
        {
            _currencySymbol = value;
            Input.CultureInfo.NumberFormat.CurrencySymbol = _currencySymbol;
        }
    }

    internal int CurrencyPositivePattern
    {
        get => _currencyPositivePattern; 
        set
        {
            _currencyPositivePattern = value;
            Input.CultureInfo.NumberFormat.CurrencyPositivePattern = _currencyPositivePattern;
        }
    }

    internal int CurrencyNegativePattern
    {
        get => _currencyNegativePattern; 
        set
        {
            _currencyNegativePattern = value;
            Input.CultureInfo.NumberFormat.CurrencyNegativePattern = _currencyNegativePattern;
        }
    }
}