using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class Карта
{
    public string НомерКарты { get; set; } = null!;

    public string СрокДействия { get; set; } = null!;

    public string ТипКарты { get; set; } = null!;

    public string ПлатежнаяСистема { get; set; } = null!;

    public DateOnly ДатаВыпуска { get; set; }

    public string КодБезопасности { get; set; } = null!;

    public decimal? Лимит { get; set; }

    public virtual КарточныйПул? КарточныйПул { get; set; }
}
