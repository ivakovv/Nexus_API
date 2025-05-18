using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class Счет
{
    public string НомерСчета { get; set; } = null!;

    public string НаименованиеСчета { get; set; } = null!;

    public string БикБанка { get; set; } = null!;

    public string КодФилиала { get; set; } = null!;

    public string ТипСчета { get; set; } = null!;

    public DateOnly ДатаОткрытия { get; set; }

    public DateOnly? ДатаЗакрытия { get; set; }

    public virtual ICollection<ДепозитныеСчета> ДепозитныеСчетаs { get; set; } = new List<ДепозитныеСчета>();

    public virtual ICollection<ИсторияТранзакций> ИсторияТранзакцийСчетОтправителяNavigations { get; set; } = new List<ИсторияТранзакций>();

    public virtual ICollection<ИсторияТранзакций> ИсторияТранзакцийСчетПолучателяNavigations { get; set; } = new List<ИсторияТранзакций>();

    public virtual ICollection<КарточныйПул> КарточныйПулs { get; set; } = new List<КарточныйПул>();

    public virtual ICollection<Кредит> Кредитs { get; set; } = new List<Кредит>();

    public virtual СчетаКлиентов? СчетаКлиентов { get; set; }
}
