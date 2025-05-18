using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class СчетаКлиентов
{
    public string НомерСчета { get; set; } = null!;

    public int Клиент { get; set; }

    public string Статус { get; set; } = null!;

    public virtual Клиент КлиентNavigation { get; set; } = null!;

    public virtual Счет НомерСчетаNavigation { get; set; } = null!;
}
