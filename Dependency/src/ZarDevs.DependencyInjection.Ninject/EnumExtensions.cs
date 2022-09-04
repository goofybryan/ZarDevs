using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Enum extensions
    /// </summary>
    public static class EnumExtensions
    {
        #region Methods

        /// <summary>
        /// Create a unique binding name from the enum value. This means the two diffent type <see cref="Enum"/> values that are the same as strings will still be unique.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>A unique string comprised of the the Type.Name_EnumValue</returns>
        public static string GetBindingName(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var name = enumValue.ToString();
            return type.Name + "_" + name;
        }

        #endregion Methods
    }
}