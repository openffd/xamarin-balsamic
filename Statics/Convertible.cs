using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Balsamic
{
    internal static class Convertible
    {
        internal static Foundation.NSString NSString<T>(this T someEnum) where T : IConvertible
        {
            return (Foundation.NSString)someEnum.String();
        }

        internal static string String<T>(this T someEnum) where T : IConvertible
        {
            string description = string.Empty;
            bool isEnum = someEnum is Enum;

            if (!isEnum)
                return description;

            Type type = someEnum.GetType();
            Array values = Enum.GetValues(type);
            foreach (int value in values)
            {
                if (value != someEnum.ToInt32(CultureInfo.InvariantCulture))
                    continue;

                if (type.GetMember(type.GetEnumName(value))[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                {
                    return descriptionAttribute.Description;
                }
            }

            return description;
        }
    }
}
