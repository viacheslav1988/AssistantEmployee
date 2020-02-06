using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantEmployee.Models.PositiveResponse
{
    enum Discounts
    {
        [DescriptionValue("Скидка 50 руб.", Value = "50 (Пятьдесят) рублей 00 копеек")]
        RUB50,
        [DescriptionValue("Скидка 100 руб.", Value = "100 (Сто) рублей 00 копеек")]
        RUB100,
        [DescriptionValue("Скидка 150 руб.", Value = "150 (Сто пятьдесят) рублей 00 копеек")]
        RUB150,
        [DescriptionValue("Скидка 200 руб.", Value = "200 (Двести) рублей 00 копеек")]
        RUB200,
        [DescriptionValue("Скидка 250 руб.", Value = "250 (Двести пятьдесят) рублей 00 копеек")]
        RUB250,
        [DescriptionValue("Скидка 300 руб.", Value = "300 (Триста) рублей 00 копеек")]
        RUB300,
        [DescriptionValue("Скидка 350 руб.", Value = "350 (Триста пятьдесят) рублей 00 копеек")]
        RUB350
    }
}