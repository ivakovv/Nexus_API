using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class ВидКредита
{
    public int КодПродукта { get; set; }

    public string Название { get; set; } = null!;

    public string Категория { get; set; } = null!;

    public decimal? МинимальнаяСумма { get; set; }

    public decimal? МаксимальнаяСумма { get; set; }

    public int МинимальныйСрок { get; set; }

    public int МаксимальныйСрок { get; set; }

    public decimal БазоваяСтавка { get; set; }

    public bool НеобходимоСтрахование { get; set; }

    public DateTime ДатаСоздания { get; set; }

    public bool Активный { get; set; }

    public string? Описание { get; set; }

    public virtual ICollection<Кредит> Кредитs { get; set; } = new List<Кредит>();
}
