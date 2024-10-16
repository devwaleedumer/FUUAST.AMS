

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AMS.SHARED.Extensions
{
    public static class EnumDescription
    {
        public static string GetDescription (this Enum value)
        {
            var field = value.GetType().GetField(value.ToString())!.GetCustomAttributes(typeof(DescriptionAttribute),false);
            if (field.Length > 0)
                return ((DescriptionAttribute)field[0]).Description;
            string result = value.ToString();
            result = Regex.Replace(result, "([a-z])([A-Z])", "$1 $2");
            result = Regex.Replace(result, "([A-Za-z])([0-9])", "$1 $2");
            result = Regex.Replace(result, "([0-9])([A-Za-z])", "$1 $2");
            result = Regex.Replace(result, "(?<!^)(?<! )([A-Z][a-z])", " $1");
            return result;
        }
        public static ReadOnlyCollection<string> GetDescriptionList(this Enum enumValue)
        {
            string result = enumValue.GetDescription();
            return new ReadOnlyCollection<string>(result.Split(',').ToList());
        }

    }
}
