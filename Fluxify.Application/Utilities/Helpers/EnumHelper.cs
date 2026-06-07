using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Utilities.Helpers
{
    public static class EnumHelper
    {
        public static TEnum Parse<TEnum>(string value, TEnum defaultValue = default(TEnum))
            where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            if (Enum.TryParse<TEnum>(value, true, out var result))
                return result;
            else
                throw new ArgumentException($"{value} is not defined!");

        }

        public static string ToEnumString<TEnum>(int value, bool useName = true)
            where TEnum : struct, Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
                throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(TEnum).Name}.");

            var enumValue = (TEnum)Enum.ToObject(typeof(TEnum), value);

            return useName ? enumValue.ToString() : value.ToString();
        }

    }
}
