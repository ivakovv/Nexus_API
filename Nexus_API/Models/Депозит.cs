using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class Депозит
{
    public string НомерДепозитногоДоговора { get; set; } = null!;

    public string Название { get; set; } = null!;

    public string ТипДепозита { get; set; } = null!;

    public decimal СуммаДепозита { get; set; }

    public string Валюта { get; set; } = null!;

    public decimal ПроцентнаяСтавка { get; set; }

    public decimal ПроцентНалога { get; set; }

    public DateOnly ДатаОткрытия { get; set; }

    public DateOnly? ДатаЗакрытия { get; set; }

    public string СпособВыплаты { get; set; } = null!;

    public int? МинимальныйСрок { get; set; }

    public DateOnly? ДатаСледующейКапитализации { get; set; }

    public virtual ДепозитныеСчета? ДепозитныеСчета { get; set; }
}
