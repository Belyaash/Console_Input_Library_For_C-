using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInput
{
    public static class GenericMethods
    {
        private static MethodInfo GetTryParseMethod(Type type)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static;
            Type[] parameterTypes = new Type[] { typeof(string), type.MakeByRefType() };
            MethodInfo method = type.GetMethod("TryParse", bindingFlags, null, parameterTypes, null);
            return method;
        }
        internal static T ReadStaticField<T>(string name, object o)
        {
            FieldInfo field = typeof(T).GetField(name, BindingFlags.Public | BindingFlags.Static);
            if (field == null)
            {
                throw new InvalidOperationException
                ("Invalid type argument for ReadStaticField<T>: " +
                 typeof(T).Name);
            }

            return (T)field.GetValue(o);
        }

        public static bool TryParse(this string s, Type type, out object result)
        {
            result = null;
            MethodInfo method = GetTryParseMethod(type);
            if (method == null)
            {
                throw new Exception();
            }
            object[] parameters = new object[] { s, null };
            bool success = (bool)method.Invoke(null, parameters);
            if (success)
            {
                result = parameters[1];
            }
            return success;
        }

        public static bool TryParse<T>(this string s, out T result)
        {
            result = default;
            bool success = s.TryParse(typeof(T), out var tempResult);
            if (success)
            {
                result = (T)tempResult;
            }
            return success;
        }
    }
}