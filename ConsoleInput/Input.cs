using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleInput.Internals;
using ConsoleInput.Internals.InputRules.CheckRules;
using ConsoleInput.Internals.Validator;

namespace ConsoleInput
{
    public static class Input
    {
        private static readonly InputOptionsHolder OptionsHolder;

        internal static readonly CultureInfo CultureInfo;
        static Input()
        {
            CultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
            OptionsHolder = new InputOptionsHolder(CultureInfo);
        }

        public static InputOptionsHolder InputOptions()
        {
            return OptionsHolder;
        }
        private static void CheckValidOfType(TypeCode tc)
        {
            switch (tc)
            {
                case TypeCode.Empty:
                case TypeCode.Object:
                case TypeCode.DBNull:
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.String:
                    throw new ArgumentOutOfRangeException(nameof(tc), tc, null);
            }
        }
        private static void PrintWelcomeMessageAndPattern(string welcome, string pattern)
        {
            Console.WriteLine(welcome);
            Console.Write(pattern);
        }


        public static TimeSpan CreateMinutesSecondsMillisecondsTimeSpan(string welcome)
        {
            PrintWelcomeMessageAndPattern(welcome, "mm:ss.mls");

            int minutes = CreateIntWithLengthRuleAndFillSpaceWithZeros(0, MinMax<int>.Range(0, 59));
            int seconds = CreateIntWithLengthRuleAndFillSpaceWithZeros(3, MinMax<int>.Range(0, 59));
            int milliseconds = CreateIntWithLengthRuleAndFillSpaceWithZeros(6, MinMax<int>.Range(0, 999));

            Console.WriteLine();
            return new TimeSpan(0, 0 ,  minutes, seconds, milliseconds);
        }

        

        public static TimeSpan CreateFullTimeSpan(string welcome)
        {
            PrintWelcomeMessageAndPattern(welcome, "dddddddd.hh:mm:ss.mls");

            int days = CreateIntWithLengthRule(0, MinMax<int>.Range(0,10000000));
            int daysLength = days.ToString().Length;
            ConsoleWriter.OverwriteCurrentLine(days + ".hh:mm:ss.ms");

            int hours = CreateIntWithLengthRuleAndFillSpaceWithZeros(daysLength + 1, MinMax<int>.Range(0, 23));
            int minutes = CreateIntWithLengthRuleAndFillSpaceWithZeros(daysLength + 4, MinMax<int>.Range(0, 59));
            int seconds = CreateIntWithLengthRuleAndFillSpaceWithZeros(daysLength + 7, MinMax<int>.Range(0, 59));
            int milliseconds = CreateIntWithLengthRuleAndFillSpaceWithZeros(daysLength + 10, MinMax<int>.Range(0, 999));

            Console.WriteLine();
            return new TimeSpan(days,hours,minutes,seconds,milliseconds);
        }


        public static DateTime CreateDate(string welcome)
        {
            PrintWelcomeMessageAndPattern(welcome, CultureInfo.DateTimeFormat.ShortDatePattern);

            string[] dateFormat = CultureInfo.DateTimeFormat.ShortDatePattern.Split(CultureInfo.DateTimeFormat.DateSeparator);
            int separatorLength = CultureInfo.DateTimeFormat.DateSeparator.Length;

            DateTime result = EnterEachDateFormatByRegionalStandardOrder(dateFormat,separatorLength);

            Console.WriteLine();
            return result;
        }

        private static DateTime EnterEachDateFormatByRegionalStandardOrder(string[] dateFormat, int separatorLength)
        {
            int currentPos = 0;
            int day = 0;
            int month = 0;
            int year = 0;

            foreach (var item in dateFormat)
            {
                switch (item[0])
                {
                    case 'd':
                        day = CreateIntWithLengthRuleAndFillSpaceWithZeros(currentPos, MinMax<int>.Range(1, 31));
                        break;
                    case 'M':
                        month = CreateIntWithLengthRuleAndFillSpaceWithZeros(currentPos, MinMax<int>.Range(1, 12));
                        break;
                    case 'y':
                        year = CreateIntWithLengthRuleAndFillSpaceWithZeros(currentPos, MinMax<int>.Range(1, 999));
                        break;
                }
                currentPos += item.Length + separatorLength;
            }
            return new DateTime(year, month, day);
        }

        private static int CreateIntWithLengthRuleAndFillSpaceWithZeros(int leftPos, MinMax<int> minMax)
        {
            var result = CreateIntWithLengthRule(leftPos, minMax);
            FillSpaceWithZeros(leftPos, result, minMax.GetMaxLength());
            return result;
        }

        private static int CreateIntWithLengthRule(int leftPos, MinMax<int> minMax)
        {
            int length = minMax.GetMaxLength();
            var rules = CreateCheckRulesForNumericInput(length);
            var result = CreateNumberOnPartOfLine<int>(minMax, null, rules, leftPos,
                leftPos + length);
            return result;
        }

        private static List<ICheckRule> CreateCheckRulesForNumericInput(int maxLength)
        {
            var rules = new List<ICheckRule>()
            {
                new LengthCheckRule(maxLength),
                new NativeDigitsCheckRule(CultureInfo.NumberFormat.NativeDigits)
            };
            return rules;
        }

        private static void FillSpaceWithZeros(int leftPos, int result, int length)
        {
            int resultLength = result.ToString().Length;
            if (resultLength == length)
                return;
            string newResult = new string('0', length - resultLength) + result;
            ConsoleWriter.OverwritePartOfCurrentLine(newResult,leftPos, leftPos + length);
        }



        /// <summary>
        /// Returns a Boolean value based on the spelling of the corresponding letter
        /// </summary>
        /// <returns> True if user pressed 'y' or 'Y'. False if user pressed 'n' or 'N'</returns>
        public static bool CreateBoolean(string welcome)
        {
            Console.WriteLine(welcome);
            return CreateBoolean();
        }
        private static bool CreateBoolean()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.KeyChar)
            {
                case 'y' or 'Y':
                    return true;
                case 'n' or 'N':
                    return false;
                default:
                    Console.Beep();
                    return CreateBoolean();
            }
        }


        
        public static T CreateNumber<T>(string welcome) where T : struct, IComparable<T>
        {
            return CreateNumber<T>(welcome, icr: null);
        }

        public static T CreateNumber<T>(string welcome, List<ICheckRule>? icr) where T : struct, IComparable<T>
        {
            MinMax<T> minMax = MinMax<T>.TypeRange();
            return CreateNumber<T>(welcome, minMax, null, icr);
        }

        public static T CreateNumber<T>(string welcome, MinMax<T> minMax) where T : struct, IComparable<T>
        {
            return CreateNumber<T>(welcome, minMax, null, null);
        }

        public static T CreateNumber<T>(string welcome, MinMax<T> minMax, List<ICheckRule>? icr) where T : struct, IComparable<T>
        {
            return CreateNumber<T>(welcome, minMax,null, icr);
        }

        public static T CreateNumber<T>(string welcome, string? format) where T : struct, IComparable<T>
        {
            return CreateNumber<T>(welcome, format, null);
        }

        public static T CreateNumber<T>(string welcome, string? format, List<ICheckRule>? icr) where T : struct, IComparable<T>
        {
            MinMax<T> minMax = MinMax<T>.TypeRange();
            return CreateNumber<T>(welcome, minMax, format, icr);
        }

        public static T CreateNumber<T>(string welcome, MinMax<T> minMax,string? format) where T : struct, IComparable<T>
        {
            return CreateNumber<T>(welcome, minMax, format, null);
        }

        public static T CreateNumber<T>(string welcome, MinMax<T> minMax,string? format, List<ICheckRule>? icr) where T : struct, IComparable<T>
        {
            Console.WriteLine(welcome);
            T number = CreateNumberOnPartOfLine(minMax, format, icr, Console.CursorLeft, Console.BufferWidth);
            Console.WriteLine();
            return number;
        }
        private static T CreateNumberOnPartOfLine<T>(MinMax<T> minMax, string? format, List<ICheckRule>? icr, int leftPos, int rightPos) 
            where T : struct, IComparable<T>
        {
            TypeCode typeCode = Type.GetTypeCode(typeof(T));
            format ??= "#,#.###;-#,#.###;0";
            IValidator validator = Validator.GetByTypeCode<T>(typeCode, CultureInfo, minMax.Min, minMax.Max);
            if (icr != null)
                validator.ReplaceCheckRules(icr);

            IInputBuffer ib = new InputBuffer(validator, CultureInfo, typeCode);
            InputInConsole<T>(format, ib, leftPos, rightPos);

            ib.Result.TryParse<T>(out var number);

            return number;
        }

        private static void InputInConsole<T>(string format, IInputBuffer ib, int leftPos, int rightPos)
            where T : IComparable<T>
        {
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey(true);
                ib.ProcessInput(cki);
                ib.PrintResultOnPartOfLine(format, leftPos, rightPos);
            } while ((cki.Key != ConsoleKey.Enter) || (!ib.IsValidResult));
        }



        public static uint InputHexadecimal(string welcome, MinMax<uint> decimalRange)
        {
            Console.WriteLine(welcome);

            IValidator validator = Validator.GetForHexadecimal(CultureInfo, decimalRange.Min, decimalRange.Max);
            IInputBuffer ib = new InputBuffer(validator, CultureInfo, TypeCode.String);
            InputInConsole<string>("X", ib, Console.CursorLeft, Console.BufferWidth);

             
            return uint.Parse(ib.Result, NumberStyles.HexNumber) ;
        }

        public static string InputHexadecimalString(string welcome, MinMax<uint> decimalRange, List<ICheckRule>? icr)
        {
            Console.WriteLine(welcome);

            IValidator validator = Validator.GetForHexadecimal(CultureInfo, decimalRange.Min, decimalRange.Max);
            if (icr != null)
                validator.ReplaceCheckRules(icr);
            IInputBuffer ib = new InputBuffer(validator, CultureInfo, TypeCode.String);
            InputInConsole<string>("X", ib, Console.CursorLeft, Console.BufferWidth);
            Console.WriteLine();

            return ib.Result;
        }
    }
}