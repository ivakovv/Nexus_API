namespace Nexus_API.Models.DTOs;

public record PaymentScheduleResponse(
    int НомерПлатежа,
    string НомерДоговора,
    DateOnly ДатаПлатежа,
    DateOnly? ДатаОплаты,
    decimal СуммаПлатежа,
    decimal ОсновнойДолг,
    decimal Проценты,
    string Статус);

public record CreditResponse(
    string НомерДоговора,
    int ВидКредита,
    string НомерСчета,
    decimal Сумма,
    decimal Ставка,
    DateOnly ДатаВыдачи,
    DateOnly? ДатаЗакрытия,
    string Статус,
    string ЦельКредита,
    IEnumerable<PaymentScheduleResponse> ГрафикПлатежей);

public record CreditCreateRequest(
    int ВидКредита,
    string НомерСчета,
    decimal Сумма,
    decimal Ставка,
    string ЦельКредита);

public record CreditUpdateRequest(
    decimal? Ставка,
    string? Статус,
    string? ЦельКредита,
    DateOnly? ДатаЗакрытия); 