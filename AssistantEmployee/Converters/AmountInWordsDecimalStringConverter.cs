using AssistantEmployee.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AssistantEmployee.Converters
{
    class AmountInWordsDecimalStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AmountInWords amountInWords = new AmountInWords();
            if (value is decimal) return amountInWords.Read((decimal)value);
            else return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string current = value as string;
            if (current != null)
            {
                current = current.OnlyNumbersToConvert();
                if (string.IsNullOrEmpty(current)) return decimal.Zero;
                else if (decimal.TryParse(current, out decimal result)) return result;
                else return Binding.DoNothing;
            }
            else return Binding.DoNothing;
        }
    }
}