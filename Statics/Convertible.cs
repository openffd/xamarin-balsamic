using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Balsamic
{
    public static class Convertible
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }
  return description;
}


    public static string String<T>(this T someEnum) where T : IConvertible
        {
            if (!(someEnum is Enum))
            {
                return string.Empty;
            }

            Type type = someEnum.GetType();
            Array array = Enum.GetValues(type);
            foreach (int item in array)
            {
                if (item != someEnum.ToInt32(CultureInfo.InvariantCulture))
                {
                    continue;
                }

                if (type.GetMember(type.GetEnumName(item))[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                {
                    return descriptionAttribute.Description;
                }
            }
        }
    }
}
