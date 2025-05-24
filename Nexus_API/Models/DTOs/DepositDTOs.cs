namespace Nexus_API.Models.DTOs;

public record DepositAccountResponse(
    string НомерДепозитногоДоговора,
    string НомерСчета,
    string Статус
);

public record DepositResponse(
    string НомерДепозитногоДоговора,
    string Название,
    string ТипДепозита,
    decimal СуммаДепозита,
    string Валюта,
    decimal ПроцентнаяСтавка,
    decimal ПроцентНалога,
    DateOnly ДатаОткрытия,
    DateOnly? ДатаЗакрытия,
    string СпособВыплаты,
    int? МинимальныйСрок,
    DateOnly? ДатаСледующейКапитализации,
    DepositAccountResponse? ДепозитныйСчет
);

public record DepositCreateRequest(
    string Название,
    string ТипДепозита,
    decimal СуммаДепозита,
    string Валюта,
    decimal ПроцентнаяСтавка,
    decimal ПроцентНалога,
    string СпособВыплаты,
    int? МинимальныйСрок,
    string НомерСчета
);

public record DepositUpdateRequest(
    string? Название,
    string? ТипДепозита,
    decimal? СуммаДепозита,
    string? Валюта,
    decimal? ПроцентнаяСтавка,
    decimal? ПроцентНалога,
    string? СпособВыплаты,
    int? МинимальныйСрок,
    DateOnly? ДатаЗакрытия,
    DateOnly? ДатаСледующейКапитализации,
    string? Статус
); 