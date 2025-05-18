using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class ИсторияТранзакций
{
    public string КодТранзакции { get; set; } = null!;

    public string? СчетОтправителя { get; set; }

    public string? СчетПолучателя { get; set; }

    public decimal Сумма { get; set; }

    public string? ВнешнийСчетОтправителя { get; set; }

    public string? БикБанкаОтправителя { get; set; }

    public string? ВнешнийСчетПолучателя { get; set; }

    public string? БикБанкаПолучателя { get; set; }

    public decimal Комиссия { get; set; }

    public DateTime ДатаСоздания { get; set; }

    public DateTime? ДатаВыполнения { get; set; }

    public string Статус { get; set; } = null!;

    public virtual Транзакция КодТранзакцииNavigation { get; set; } = null!;

    public virtual Счет? СчетОтправителяNavigation { get; set; }

    public virtual Счет? СчетПолучателяNavigation { get; set; }
}
