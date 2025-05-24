namespace Nexus_API.Models.DTOs;

public record CardResponse(
    string НомерКарты,
    DateOnly ДатаВыпуска,
    string КодБезопасности,
    decimal? Лимит,
    string ПлатежнаяСистема,
    string СрокДействия,
    string ТипКарты,
    string? Статус,
    string НомерСчета);

public record CardCreateRequest(
    string НомерКарты,
    string КодБезопасности,
    decimal? Лимит,
    string ПлатежнаяСистема,
    string СрокДействия,
    string ТипКарты,
    string НомерСчета,
    string? Статус = "Активна");

public record CardUpdateRequest(
    string? НомерКарты,
    DateOnly? ДатаВыпуска,
    string КодБезопасности,
    decimal? Лимит,
    string? ПлатежнаяСистема,
    string? СрокДействия,
    string? ТипКарты,
    string? Статус,
    string? НомерСчета);