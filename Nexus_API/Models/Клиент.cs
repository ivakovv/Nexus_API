using System;
using System.Collections.Generic;

namespace Nexus_API.Models;

public partial class Клиент
{
    public int Id { get; set; }

    public string СерияПаспорта { get; set; } = null!;

    public string НомерПаспорта { get; set; } = null!;

    public string Фамилия { get; set; } = null!;

    public string Имя { get; set; } = null!;

    public string? Отчество { get; set; }

    public string Пол { get; set; } = null!;

    public DateOnly ДатаВыдачиПаспорта { get; set; }

    public string КемВыданПаспорт { get; set; } = null!;

    public string МестоРождения { get; set; } = null!;

    public DateOnly ДатаРождения { get; set; }

    public string АдресРегистрации { get; set; } = null!;

    public string? АдресПроживания { get; set; }

    public string? СемейноеПоложение { get; set; }

    public string? НомерТелефона { get; set; }

    public string? Email { get; set; }

    public string Инн { get; set; } = null!;

    public string Снилс { get; set; } = null!;

    public string? ИсточникДохода { get; set; }

    public string? Работодатель { get; set; }

    public decimal? УровеньДохода { get; set; }

    public string СтатусКлиента { get; set; } = null!;

    public DateOnly ДатаРегистрацииВБанке { get; set; }

    public string? ДополнительныеСведения { get; set; }

    public virtual ICollection<СчетаКлиентов> СчетаКлиентовs { get; set; } = new List<СчетаКлиентов>();
}
