using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInput.Internals
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
            object tempResult;
            bool success = s.TryParse(typeof(T), out tempResult);
            if (success)
            {
                result = (T)tempResult;
            }
            return success;
        }
    }
}