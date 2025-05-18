using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class Транзакция
{
    public string КодТранзакции { get; set; } = null!;

    public string ТипТранзакции { get; set; } = null!;

    public string Направление { get; set; } = null!;

    public virtual ИсторияТранзакций? ИсторияТранзакций { get; set; }
}
