using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInput
{
    public static class Input
    {
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
            Type type = typeof(T);
            TypeCode typeCode = Type.GetTypeCode(type);
            CheckValidOfType(typeCode);
            Console.WriteLine(welcome);

            var validator = Validator<T>.GetByTypeCode(typeCode);
            var ib = new InputBuffer(validator);

            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey(true);
                ib.ProcessInput(cki);
                ib.PrintCurrentResult();

            } while (cki.Key != ConsoleKey.Enter);
            return ParseToT();
        }
    }
}