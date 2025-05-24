using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class Кредит
{
    public string НомерДоговора { get; set; } = null!;

    public string НомерСчета { get; set; } = null!;

    public int ВидКредита { get; set; }

    public decimal Сумма { get; set; }

    public DateOnly ДатаВыдачи { get; set; }

    public DateOnly? ДатаЗакрытия { get; set; }

    public decimal Ставка { get; set; }

    public string? ЦельКредита { get; set; }

    public string Статус { get; set; } = null!;

    public virtual ВидКредита ВидКредитаNavigation { get; set; } = null!;

    public virtual ICollection<ГрафикПлатежей> ГрафикПлатежейs { get; set; } = new List<ГрафикПлатежей>();

    public virtual Счет НомерСчетаNavigation { get; set; } = null!;
}
