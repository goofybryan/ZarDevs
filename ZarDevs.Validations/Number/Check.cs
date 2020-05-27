using System;

namespace ZarDevs.Validations.Number
{
    public static class Check
    {
        #region Methods

        public static bool IsNumber(object expression)
        {
            if (expression == null || expression is DateTime)
                return false;

            if (expression is short || expression is int || expression is long || expression is decimal || expression is float || expression is double || expression is bool)
                return true;

            return double.TryParse(expression.ToString(), out _);
        }

        #endregion Methods
    }
}