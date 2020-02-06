using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantEmployee.Models.PositiveResponse
{
    class PositiveResponseModel
    {

        #region Свойства документа

        //Номер договора
        public string ContractNumber { get; set; } = string.Empty;

        //Дата договора
        public DateTime? ContractDate { get; set; }

        //Ф.И.О.
        public string FullName { get; set; } = string.Empty;

        //Пол
        public Gender Gender { get; set; } = Gender.Male;

        //Адрес
        public string Address { get; set; } = string.Empty;

        //Исходящий номер
        public string OutgoingNumber { get; set; } = string.Empty;

        //Дата ответа
        public DateTime? OutgoingDate { get; set; }

        //Заявление на расторжение договора
        public bool StatementTermination { get; set; }

        //Дата расторжения договора
        public DateTime? ContractTerminationDate { get; set; }

        //Сумма долга
        public decimal AmountDebt { get; set; }

        //Заявление на перерасчет
        public bool StatementRecalculation { get; set; } = true;

        //Дата заявления на перерасчет
        public DateTime? StatementRecalculationDate { get; set; }

        //Тип скидки
        public DiscountType DiscountType { get; set; } = DiscountType.Discount;

        //Список скидок
        public Discounts Discounts { get; set; } = Discounts.RUB50;

        //Дата начала скидки
        public DateTime? StartDateDiscount { get; set; }

        //Дата окончания скидки
        public DateTime? EndDateDiscount { get; set; }

        //Сумма перерасчета
        public decimal AmountRecalculation { get; set; }

        #endregion

    }
}