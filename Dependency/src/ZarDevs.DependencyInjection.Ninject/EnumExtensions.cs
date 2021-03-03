using System;

namespace ZarDevs.DependencyInjection
{
    internal static class EnumExtensions
    {
        #region Methods

        public static string GetBindingName(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var name = enumValue.ToString();
            return type.Name + "_" + name;
        }

        #endregion Methods
    }
}