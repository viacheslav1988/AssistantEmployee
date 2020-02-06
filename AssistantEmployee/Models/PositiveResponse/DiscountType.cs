using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistantEmployee.Models.PositiveResponse
{
    enum DiscountType
    {
        [DescriptionValue("Скидка")]
        Discount,
        [DescriptionValue("Перерасчёт")]
        Recalculation,
        [DescriptionValue("Скидка и перерасчёт")]
        DiscountAndRecalculation
    }
}