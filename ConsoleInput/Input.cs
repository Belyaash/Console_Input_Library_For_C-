﻿using System;
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
        private static readonly CultureInfo CultureInfo;
        static Input()
        {
            CultureInfo = new CultureInfo(CultureInfo.CurrentCulture.LCID);
        }

        public static CultureInfo InputOptions()
        {
            return CultureInfo;
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