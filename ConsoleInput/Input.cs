using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleInput.Internals;

namespace ConsoleInput
{
    public static class Input
    {
        internal static CultureInfo cultureInfo;
        static Input()
        {
            cultureInfo = CultureInfo.CurrentCulture;
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

        public static T CreateNumber<T>(string welcome) where T : struct, IComparable<T>
        {
            MinMax<T> minMax = MinMax<T>.TypeRange();
            return CreateNumber<T>(welcome, minMax, null);
        }

        public static T CreateNumber<T>(string welcome, MinMax<T> minMax) where T : struct, IComparable<T>
        {
            return CreateNumber<T>(welcome, minMax, null);
        }

        public static T CreateNumber<T>(string welcome, string? format) where T : struct, IComparable<T>
        {
            MinMax<T> minMax = MinMax<T>.TypeRange();
            return CreateNumber<T>(welcome, minMax, format);
        }

        public static T CreateNumber<T>(string welcome, MinMax<T> minMax,string? format) where T : struct, IComparable<T>
        {
            Type type = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(type);
            CheckValidOfType(typeCode);

            Console.WriteLine(welcome);

            format ??= "#,#.###;-#,#.###;0";
            IValidator validator = Validator.GetByTypeCode<T>(typeCode, cultureInfo, minMax.Min, minMax.Max);
            IInputBuffer ib = new InputBuffer(validator, cultureInfo, typeCode);

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