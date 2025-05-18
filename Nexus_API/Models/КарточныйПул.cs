using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class КарточныйПул
{
    public string НомерКарты { get; set; } = null!;

    public string НомерСчета { get; set; } = null!;

    public string Статус { get; set; } = null!;

    public virtual Карта НомерКартыNavigation { get; set; } = null!;

    public virtual Счет НомерСчетаNavigation { get; set; } = null!;
}
