using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Balsamic
{
    public static class Convertible
    {
        public static string String<T>(this T someEnum) where T : IConvertible
        {
            var isEnum = someEnum is Enum;
            if (!isEnum)
            {
                return string.Empty;
            }

            Type type = someEnum.GetType();
            Array values = Enum.GetValues(type);
            foreach (int value in values)
            {
                if (value != someEnum.ToInt32(CultureInfo.InvariantCulture))
                {
                    continue;
                }

                if (type.GetMember(type.GetEnumName(value))[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                {
                    return descriptionAttribute.Description;
                }
            }

            return string.Empty;
        }
    }
}
