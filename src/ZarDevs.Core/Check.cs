using ZarDevs.Core.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ZarDevs.Core
{
    public static class Check
    {
        #region Methods

        public static void IsNotNullAndThrow<TException>(object value, params object[] exceptionArgs) where TException : Exception
        {
            if (Equals(value, null))
            {
                throw Create.New<TException>(exceptionArgs);
            }
        }

        public static T IsNotNull<T>(T value, string paramName)
        {
            return !Equals(value, null) ? value : throw new ArgumentNullException(paramName);
        }

        public static string IsNotNullOrEmpty(string value, string paramName)
        {
            IsNotNull(value, paramName);
            return !string.IsNullOrEmpty(value) ? value : throw new ArgumentException(paramName);
        }

        public static IEnumerable<T> IsNotNullOrEmpty<T>(IEnumerable<T> enumerable, string paramName)
        {
            IsNotNull(enumerable, paramName);
            return enumerable.Any() ? enumerable : throw new InvalidOperationException($"{paramName} cannot be an empty enumerable".ToString(CultureInfo.CurrentCulture));
        }

        public static bool IsNumber(object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is short || Expression is int || Expression is long || Expression is decimal || Expression is float || Expression is double || Expression is bool)
                return true;

            try
            {
                if (Expression is string)
                    double.Parse(Expression as string);
                else
                    double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false

            return false;
        }

        public static void IsValid(Func<bool> validation, string message)
        {
            IsValid<InvalidOperationException>(validation, message);
        }

        public static void IsValid<TException>(Func<bool> validation, string message) where TException : Exception
        {
            IsValid<TException>(validation, new object[] { message });
        }

        public static void IsValid<TException>(Func<bool> validation, params object[] exceptionArgs) where TException : Exception
        {
            if (validation())
            {
                return;
            }

            var exception = Create.New<TException>(exceptionArgs);
            throw exception;
        }

        #endregion Methods
    }
}
