using AssistantEmployee.ViewModels.Workspaces.PositiveResponse;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace AssistantEmployee.Validations
{
    class ValidatingDates : ValidationRule
    {
        const string ERROR_1 = "Начальная дата не определена";
        const string ERROR_2 = "Выбранные даты меньше начальной даты";
        const string ERROR_3 = "Некорректный диапазон дат";

        public string StartDate { get; set; }
        public string StartRangeDate { get; set; }
        public string EndRangeDate { get; set; }

        public double DateRangeFrom { get; set; }
        public double DateRangeTo { get; set; }

        public string ErrorText1 { get; set; }
        public string ErrorText2 { get; set; }
        public string ErrorText3 { get; set; }

        T GetPropertyValue<T>(object source, string propertyName)
        {
            try
            {
                return (T)source.GetType().GetProperty(propertyName).GetValue(source);
            }
            catch
            {
                return default(T);
            }
        }
        
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            PositiveResponseViewModel viewModel = (value as BindingGroup)?.Items[0] as PositiveResponseViewModel;
            if (viewModel != null)
            {
                DateTime? startDate = GetPropertyValue<DateTime?>(viewModel, StartDate);
                DateTime? startRangeDate = GetPropertyValue<DateTime?>(viewModel, StartRangeDate);
                DateTime? endRangeDate = GetPropertyValue<DateTime?>(viewModel, EndRangeDate);

                if (startDate == null && startRangeDate != null && endRangeDate != null)
                    return new ValidationResult(false, ErrorText1 == null ? ERROR_1 : ErrorText1);

                if (startDate != null && startRangeDate != null && endRangeDate != null)
                {
                    if (((TimeSpan)(startRangeDate - startDate)).Days < 0 || ((TimeSpan)(endRangeDate - startDate)).Days < 0)
                        return new ValidationResult(false, ErrorText2 == null ? ERROR_2 : ErrorText2);
                    if (DateRangeFrom >= ((TimeSpan)(endRangeDate - startRangeDate)).Days || ((TimeSpan)(endRangeDate - startRangeDate)).Days >= DateRangeTo)
                        return new ValidationResult(false, ErrorText3 == null ? ERROR_3 : ErrorText3);
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}