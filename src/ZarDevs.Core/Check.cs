using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ZarDevs.Core
{
    public static class Check
    {
        #region Methods

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

        #endregion Methods
    }
}
