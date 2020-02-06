using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AssistantEmployee.Validations
{
    class ValidatingText : ValidationRule
    {
        const string ERROR = "Ошибка данных";

        public string Pattern { get; set; }

        public string ErrorText { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string && string.IsNullOrEmpty(Pattern) || string.IsNullOrEmpty((string)value) || Regex.IsMatch((string)value, Pattern)) return ValidationResult.ValidResult;
            else return new ValidationResult(false, string.IsNullOrEmpty(ErrorText) ? ERROR : ErrorText);
        }
    }
}