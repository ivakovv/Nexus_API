using Microsoft.EntityFrameworkCore;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Services;

public class CreditService : ICreditService
{
    private readonly NexusContext _context;

    public CreditService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CreditResponse>> GetAllCreditsAsync()
    {
        var кредиты = await _context.Кредитs
            .Include(k => k.ГрафикПлатежейs)
            .AsNoTracking()
            .ToListAsync();

        return кредиты.Select(КредитВОтвет);
    }

    public async Task<CreditResponse> GetCreditByNumberAsync(string номерДоговора)
    {
        var кредит = await _context.Кредитs
            .Include(k => k.ГрафикПлатежейs)
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.НомерДоговора == номерДоговора);

        if (кредит is null)
            throw new KeyNotFoundException($"Кредит с номером договора {номерДоговора} не найден");

        return КредитВОтвет(кредит);
    }

    public async Task<CreditResponse> CreateCreditAsync(CreditCreateRequest creditDto)
    {
        var новыйКредит = new Кредит
        {
            НомерДоговора = GenerateCreditNumber(),
            ВидКредита = creditDto.ВидКредита,
            НомерСчета = creditDto.НомерСчета,
            Сумма = creditDto.Сумма,
            Ставка = creditDto.Ставка,
            ДатаВыдачи = DateOnly.FromDateTime(DateTime.Now),
            Статус = "Активен",
            ЦельКредита = creditDto.ЦельКредита
        };

        await _context.Кредитs.AddAsync(новыйКредит);
        await _context.SaveChangesAsync();

        return await GetCreditByNumberAsync(новыйКредит.НомерДоговора);
    }

    public async Task UpdateCreditAsync(string номерДоговора, CreditUpdateRequest creditDto)
    {
        var кредит = await _context.Кредитs.FindAsync(номерДоговора);

        if (кредит is null)
            throw new KeyNotFoundException($"Кредит с номером договора {номерДоговора} не найден");

        if (creditDto.Ставка.HasValue)
            кредит.Ставка = creditDto.Ставка.Value;

        if (creditDto.Статус != null)
            кредит.Статус = creditDto.Статус;

        if (creditDto.ЦельКредита != null)
            кредит.ЦельКредита = creditDto.ЦельКредита;

        if (creditDto.ДатаЗакрытия.HasValue)
            кредит.ДатаЗакрытия = creditDto.ДатаЗакрытия.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCreditAsync(string номерДоговора)
    {
        var кредит = await _context.Кредитs.FindAsync(номерДоговора);

        if (кредит is null)
            throw new KeyNotFoundException($"Кредит с номером договора {номерДоговора} не найден");

        _context.Кредитs.Remove(кредит);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CreditResponse>> SearchCreditsAsync(string searchTerm)
    {
        var кредиты = await _context.Кредитs
            .Include(k => k.ГрафикПлатежейs)
            .AsNoTracking()
            .Where(k =>
                k.НомерДоговора.Contains(searchTerm) ||
                k.НомерСчета.Contains(searchTerm) ||
                k.ЦельКредита.Contains(searchTerm))
            .ToListAsync();

        return кредиты.Select(КредитВОтвет);
    }

    public async Task<IEnumerable<CreditResponse>> GetCreditsByStatusAsync(string статус)
    {
        var кредиты = await _context.Кредитs
            .Include(k => k.ГрафикПлатежейs)
            .AsNoTracking()
            .Where(k => k.Статус == статус)
            .ToListAsync();

        return кредиты.Select(КредитВОтвет);
    }

    public async Task<IEnumerable<CreditResponse>> GetCreditsByTypeAsync(int видКредита)
    {
        var кредиты = await _context.Кредитs
            .Include(k => k.ГрафикПлатежейs)
            .AsNoTracking()
            .Where(k => k.ВидКредита == видКредита)
            .ToListAsync();

        return кредиты.Select(КредитВОтвет);
    }

    public async Task<IEnumerable<PaymentScheduleResponse>> GetPaymentScheduleAsync(string номерДоговора)
    {
        var график = await _context.ГрафикПлатежейs
            .AsNoTracking()
            .Where(g => g.НомерДоговора == номерДоговора)
            .OrderBy(g => g.НомерПлатежа)
            .ToListAsync();

        return график.Select(ПлатежВОтвет);
    }

    public async Task<IEnumerable<CreditResponse>> GetCreditsByAccountAsync(string номерСчета)
    {
        var кредиты = await _context.Кредитs
            .Include(k => k.ГрафикПлатежейs)
            .AsNoTracking()
            .Where(k => k.НомерСчета == номерСчета && k.Статус == "Активен")
            .ToListAsync();

        return кредиты.Select(кредит => new CreditResponse(
            кредит.НомерДоговора,
            кредит.ВидКредита,
            кредит.НомерСчета,
            кредит.Сумма,
            кредит.Ставка,
            кредит.ДатаВыдачи,
            кредит.ДатаЗакрытия,
            кредит.Статус ?? "Неизвестно",
            кредит.ЦельКредита ?? string.Empty,
            кредит.ГрафикПлатежейs?.Select(платеж => new PaymentScheduleResponse(
                платеж.НомерПлатежа,
                платеж.НомерДоговора,
                платеж.ДатаПлатежа,
                платеж.ДатаОплаты,
                платеж.СуммаПлатежа,
                платеж.ОсновнойДолг,
                платеж.Проценты,
                платеж.Статус ?? "Неоплачен"
            )) ?? Enumerable.Empty<PaymentScheduleResponse>()
        ));
    }

    private static CreditResponse КредитВОтвет(Кредит кредит)
    {
        return new CreditResponse(
            кредит.НомерДоговора,
            кредит.ВидКредита,
            кредит.НомерСчета,
            кредит.Сумма,
            кредит.Ставка,
            кредит.ДатаВыдачи,
            кредит.ДатаЗакрытия,
            кредит.Статус ?? "Неизвестно",
            кредит.ЦельКредита ?? string.Empty,
            кредит.ГрафикПлатежейs?.Select(ПлатежВОтвет) ?? Enumerable.Empty<PaymentScheduleResponse>());
    }

    private static PaymentScheduleResponse ПлатежВОтвет(ГрафикПлатежей платеж)
    {
        return new PaymentScheduleResponse(
            платеж.НомерПлатежа,
            платеж.НомерДоговора,
            DateOnly.FromDateTime(DateTime.Now),
            платеж.ДатаОплаты,
            платеж.СуммаПлатежа,
            платеж.ОсновнойДолг,
            платеж.Проценты,
            платеж.Статус ?? "Неоплачен");
    }

    private static string GenerateCreditNumber()
    {
        return $"CR{DateTime.Now:yyyyMMdd}{Random.Shared.Next(100000, 999999)}";
    }
}