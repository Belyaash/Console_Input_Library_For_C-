using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleInput.Internals;
using ConsoleInput.Internals.InputRules.CheckRules;

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


        public static TimeSpan CreateMinutesSecondsMillisecondsTimeSpan(string welcome)
        {
            Console.WriteLine(welcome);

            int minutes = CreateNumber<int>("Enter a minutes", MinMax<int>.Range(0, 59));
            int seconds = CreateNumber<int>("Enter a seconds", MinMax<int>.Range(0, 59));
            int milliseconds = CreateNumber<int>("Enter a milliseconds", MinMax<int>.Range(0, 999));

            return new TimeSpan(0, 0 ,  minutes, seconds, milliseconds);
        }

        public static TimeSpan CreateFullTimeSpan(string welcome)
        {
            Console.WriteLine(welcome);

            int days = CreateNumber<int>("Enter a days", MinMax<int>.HigherThan(0));
            int hours = CreateNumber<int>("Enter a hours", MinMax<int>.Range(0, 23));
            int minutes = CreateNumber<int>("Enter a minutes", MinMax<int>.Range(0, 59));
            int seconds = CreateNumber<int>("Enter a seconds", MinMax<int>.Range(0, 59));
            int milliseconds = CreateNumber<int>("Enter a milliseconds", MinMax<int>.Range(0, 999));

            return new TimeSpan(days,hours,minutes,seconds,milliseconds);
        }


        public static DateTime CreateDate(string welcome)
        {
            Console.WriteLine(welcome);
            ConsoleWriter.OverwriteCurrentLine(CultureInfo.DateTimeFormat.ShortDatePattern);

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
                List<ICheckRule> rules;
                if (item[0] == 'd')
                {
                    rules = CreateCheckRulesForDate(2);
                    day = CreateNumberOnPartOfLine<int>(MinMax<int>.Range(1, 31), null, rules, currentPos,
                        currentPos + item.Length);
                }

                if (item[0] == 'M')
                {
                    rules = CreateCheckRulesForDate(2);
                    month = CreateNumberOnPartOfLine<int>(MinMax<int>.Range(1, 12), null, rules, currentPos,
                        currentPos + item.Length);
                }

                if (item[0] == 'y')
                {
                    rules = CreateCheckRulesForDate(4);
                    year = CreateNumberOnPartOfLine<int>(MinMax<int>.Range(1, 9999), null, rules, currentPos,
                        currentPos + item.Length);
                }

                currentPos += item.Length + separatorLength;
            }

            return new DateTime(year, month, day);
        }

        private static List<ICheckRule> CreateCheckRulesForDate(int maxLength)
        {
            var rules = new List<ICheckRule>()
            {
                new LengthCheckRule(maxLength),
                new NativeDigitsCheckRule(CultureInfo.NumberFormat.NativeDigits)
            };
            return rules;
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
                validator.AddCheckRules(icr);

            IInputBuffer ib = new InputBuffer(validator, CultureInfo, typeCode);
            InputInConsole<T>(format, ib, leftPos, rightPos);

            ib.Result.TryParse<T>(out var number);

            return number;
        }

        private static void InputInConsole<T>(string format, IInputBuffer ib, int leftPos, int rightPos)
            where T : struct, IComparable<T>
        {
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey(true);
                ib.ProcessInput(cki);
                ib.PrintResultOnPartOfLine(format, leftPos, rightPos);
            } while ((cki.Key != ConsoleKey.Enter) || (!ib.IsValidResult));
        }
    }
}