using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class ГрафикПлатежей
{
    public int НомерПлатежа { get; set; }

    public string НомерДоговора { get; set; } = null!;

    public DateOnly ДатаПлатежа { get; set; }

    public decimal СуммаПлатежа { get; set; }

    public decimal ОсновнойДолг { get; set; }

    public decimal Проценты { get; set; }

    public string Статус { get; set; } = null!;

    public DateOnly? ДатаОплаты { get; set; }

    public virtual Кредит НомерДоговораNavigation { get; set; } = null!;
}
