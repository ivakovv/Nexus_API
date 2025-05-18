using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class ДепозитныеСчета
{
    public string НомерДепозитногоДоговора { get; set; } = null!;

    public string НомерСчета { get; set; } = null!;

    public string Статус { get; set; } = null!;

    public virtual Депозит НомерДепозитногоДоговораNavigation { get; set; } = null!;

    public virtual Счет НомерСчетаNavigation { get; set; } = null!;
}
