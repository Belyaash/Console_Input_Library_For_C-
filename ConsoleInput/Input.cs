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
        internal static InputOptionsHolder OptionsHolder;

        internal static CultureInfo CultureInfo;
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

        public static DateTime CreateDate(string welcome)
        {
            ConsoleWriter.OverwriteCurrentLine(CultureInfo.DateTimeFormat.ShortDatePattern);
            string[] dateFormat = CultureInfo.DateTimeFormat.ShortDatePattern.Split(CultureInfo.DateTimeFormat.DateSeparator);
            int day = 0;
            int month = 0;
            int year = 0;
            int currentpos = 0;
            int separatorLength = CultureInfo.DateTimeFormat.DateSeparator.Length;
            List<ICheckRule> rules = new List<ICheckRule>();
            foreach (var item in dateFormat)
            {
                if (item[0] == 'd')
                {
                    rules = new List<ICheckRule>()
                    {
                        new LengthCheckRule(2),
                        new NativeDigitsCheckRule(CultureInfo.NumberFormat.NativeDigits)
                    };
                    day = CreateNumberOnPartOfLine<int>(MinMax<int>.Range(1, 31), null, rules, currentpos,
                        currentpos + item.Length);
                }

                if (item[0] == 'M')
                {
                    rules = new List<ICheckRule>()
                    {
                        new LengthCheckRule(2),
                        new NativeDigitsCheckRule(CultureInfo.NumberFormat.NativeDigits)
                    };
                    month = CreateNumberOnPartOfLine<int>(MinMax<int>.Range(1, 12), null, rules, currentpos,
                        currentpos + item.Length);
                }

                if (item[0] == 'y')
                {
                    rules = new List<ICheckRule>()
                    {
                        new LengthCheckRule(4),
                        new NativeDigitsCheckRule(CultureInfo.NumberFormat.NativeDigits)
                    };
                    year = CreateNumberOnPartOfLine<int>(MinMax<int>.Range(1, 9999), null, rules, currentpos,
                        currentpos + item.Length);
                }
                currentpos += item.Length + separatorLength;
            }

            Console.WriteLine();
            return new DateTime(year, month, day);
        }

        private static T CreateNumberOnPartOfLine<T>(MinMax<T> minMax, string? format, List<ICheckRule>? icr, int leftPos, int rightPos) where T : struct, IComparable<T>
        {
            Type type = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(type);

            format ??= "#,#.###;-#,#.###;0";

            IValidator validator = Validator.GetByTypeCode<T>(typeCode, CultureInfo, minMax.Min, minMax.Max);
            if (icr != null) 
                validator.AddCheckRules(icr);
            
            IInputBuffer ib = new InputBuffer(validator, CultureInfo, typeCode);

            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey(true);
                ib.ProcessInput(cki);
                ib.PrintResultOnPartOfLine(format, leftPos, rightPos);

            } while ((cki.Key != ConsoleKey.Enter)||(!ib.IsValidResult));

            bool isDone = GenericMethods.TryParse<T>(ib.Result, out var number);
            return number;
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
            Type type = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(type);
            CheckValidOfType(typeCode);

            Console.WriteLine(welcome);

            format ??= "#,#.###;-#,#.###;0";

            IValidator validator = Validator.GetByTypeCode<T>(typeCode, CultureInfo, minMax.Min, minMax.Max);
            if (icr != null) 
                validator.AddCheckRules(icr);
            
            IInputBuffer ib = new InputBuffer(validator, CultureInfo, typeCode);

            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey(true);
                ib.ProcessInput(cki);
                ib.PrintCurrentResult(format);

            } while ((cki.Key != ConsoleKey.Enter)||(!ib.IsValidResult));

            bool isDone = GenericMethods.TryParse<T>(ib.Result, out var number);
            Console.WriteLine();
            return number;
        }
    }
}