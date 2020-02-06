using AssistantEmployee.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AssistantEmployee.Converters
{
    class EnumDescriptionValueConverter : IValueConverter
    {
        enum SearchType { MemberInfo, FieldInfo }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Enum ? GetDescriptionValue((Enum)value, parameter as string) : Binding.DoNothing;

        private object GetDescriptionValue(Enum value, string parameter)
        {
            DescriptionValueAttribute attribute;
            switch(parameter)
            {
                case "value":
                    attribute = SearchAttribute(value, SearchType.MemberInfo);
                    return attribute?.Value ?? value.ToString();
                case null:
                    attribute = SearchAttribute(value, SearchType.MemberInfo);
                    return attribute?.Description ?? value.ToString();
                default:
                    attribute = SearchAttribute(value, SearchType.FieldInfo, parameter);
                    return attribute?.Description ?? value.ToString();
            }
        }

        private DescriptionValueAttribute SearchAttribute(Enum value, SearchType searchType, string parameter = "")
        {
            DescriptionValueAttribute attribute = null;
            Type type = value.GetType();
            switch(searchType)
            {
                case SearchType.MemberInfo:
                    MemberInfo[] memberInfo = type.GetMember(value.ToString());
                    if (memberInfo != null && memberInfo.Length > 0)
                    {
                        object[] customAttributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionValueAttribute), false);
                        if (customAttributes != null && customAttributes.Length > 0)
                        {
                            attribute = customAttributes.FirstOrDefault() as DescriptionValueAttribute;
                        }
                    }
                    break;
                case SearchType.FieldInfo:
                    FieldInfo fieldInfo = type.GetField(parameter);
                    if (fieldInfo != null)
                    {
                        object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionValueAttribute), false);
                        if (customAttributes != null && customAttributes.Length > 0)
                        {
                            attribute = customAttributes.FirstOrDefault() as DescriptionValueAttribute;
                        }
                    }
                    break;
            }
            return attribute;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
    }
}