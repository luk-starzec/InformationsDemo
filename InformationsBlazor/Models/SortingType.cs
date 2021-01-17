using System;
using System.ComponentModel;

namespace InformationsBlazor.Models
{
    public enum SortingType
    {
        [Description("od najnowszych")]
        FromNewest = 1,
        [Description("alfabetycznie")]
        Alphabetical = 2,
    }

    public static class SortingTypeExtensions
    {
        public static string Description(this Enum value)
        {
            var enumType = value.GetType();
            if (Enum.IsDefined(enumType, value))
            {
                var field = enumType.GetField(value.ToString());
                var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

                return attributes.Length == 0
                    ? value.ToString()
                    : ((DescriptionAttribute)attributes[0]).Description;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
