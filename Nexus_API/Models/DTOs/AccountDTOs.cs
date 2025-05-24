namespace Nexus_API.Models.DTOs;

public record AccountResponse(
    string НомерСчета,
    string ТипСчета,
    string НаименованиеСчета,
    string БикБанка,
    string КодФилиала,
    DateOnly ДатаОткрытия,
    DateOnly? ДатаЗакрытия);

public record AccountCreateRequest(
    string ТипСчета,
    string НаименованиеСчета,
    string БикБанка,
    string КодФилиала,
    int КлиентId);

public record AccountUpdateRequest(
    string? НаименованиеСчета,
    string? БикБанка,
    string? КодФилиала,
    DateOnly? ДатаЗакрытия);