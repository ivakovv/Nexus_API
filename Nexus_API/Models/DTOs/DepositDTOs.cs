namespace Nexus_API.Models.DTOs;

public record DepositResponse(
    string НомерДепозитногоДоговора,
    string Валюта,
    DateOnly ДатаОткрытия,
    DateOnly ДатаЗакрытия,
    DateOnly ДатаСледующейКапитализации,
    int? МинимальныйСрок,
    string Название,
    decimal ПроцентНалога,
    decimal ПроцентнаяСтавка,
    string СпособВыплаты,
    decimal СуммаДепозита,
    string ТипДепозита);

public record DepositCreateRequest(
    string Валюта,
    int? МинимальныйСрок,
    string Название,
    decimal ПроцентНалога,
    decimal ПроцентнаяСтавка,
    string СпособВыплаты,
    decimal СуммаДепозита,
    string ТипДепозита);

public record DepositUpdateRequest(
    DateOnly ДатаЗакрытия,
    DateOnly ДатаСледующейКапитализации,
    string? СпособВыплаты,
    decimal? ПроцентнаяСтавка);