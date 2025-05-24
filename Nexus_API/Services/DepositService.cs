using Microsoft.EntityFrameworkCore;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Services;

public class DepositService : IDepositService
{
    private readonly NexusContext _context;

    public DepositService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepositResponse>> GetAllDepositsAsync()
    {
        var текущаяДата = DateOnly.FromDateTime(DateTime.Now);

        var депозиты = await _context.Депозитs
            .Include(d => d.ДепозитныеСчета)
            .Where(d => d.ДепозитныеСчета != null &&
                       d.ДепозитныеСчета.Статус == "Активен" &&
                       d.ДатаОткрытия <= текущаяДата)
            .AsNoTracking()
            .ToListAsync();

        return депозиты.Select(ДепозитВОтвет);
    }

    public async Task<DepositResponse> GetDepositByNumberAsync(string номерДепозитногоДоговора)
    {
        var депозит = await _context.Депозитs
            .Include(d => d.ДепозитныеСчета)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.НомерДепозитногоДоговора == номерДепозитногоДоговора);

        if (депозит is null)
            throw new KeyNotFoundException($"Депозит с номером договора {номерДепозитногоДоговора} не найден");

        return ДепозитВОтвет(депозит);
    }

    public async Task<DepositResponse> CreateDepositAsync(DepositCreateRequest depositDto)
    {
        var новыйДепозит = new Депозит
        {
            Название = depositDto.Название,
            ТипДепозита = depositDto.ТипДепозита,
            СуммаДепозита = depositDto.СуммаДепозита,
            Валюта = depositDto.Валюта,
            ПроцентнаяСтавка = depositDto.ПроцентнаяСтавка,
            ПроцентНалога = depositDto.ПроцентНалога,
            ДатаОткрытия = DateOnly.FromDateTime(DateTime.Now),
            СпособВыплаты = depositDto.СпособВыплаты,
            МинимальныйСрок = depositDto.МинимальныйСрок,
            ДатаСледующейКапитализации = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            ДатаЗакрытия = DateOnly.FromDateTime(DateTime.Now.AddYears(1))
        };

        await _context.Депозитs.AddAsync(новыйДепозит);
        await _context.SaveChangesAsync();

        var депозитныйСчет = new ДепозитныеСчета
        {
            НомерДепозитногоДоговора = новыйДепозит.НомерДепозитногоДоговора,
            НомерСчета = depositDto.НомерСчета,
            Статус = "Активен"
        };
        await _context.ДепозитныеСчетаs.AddAsync(депозитныйСчет);
        await _context.SaveChangesAsync();

        return await GetDepositByNumberAsync(новыйДепозит.НомерДепозитногоДоговора);
    }

    public async Task UpdateDepositAsync(string номерДепозитногоДоговора, DepositUpdateRequest depositDto)
    {
        var депозит = await _context.Депозитs.Include(d => d.ДепозитныеСчета).FirstOrDefaultAsync(d => d.НомерДепозитногоДоговора == номерДепозитногоДоговора);
        if (депозит is null)
            throw new KeyNotFoundException($"Депозит с номером договора {номерДепозитногоДоговора} не найден");

        if (depositDto.Название != null)
            депозит.Название = depositDto.Название;
        if (depositDto.ТипДепозита != null)
            депозит.ТипДепозита = depositDto.ТипДепозита;
        if (depositDto.СуммаДепозита.HasValue)
            депозит.СуммаДепозита = depositDto.СуммаДепозита.Value;
        if (depositDto.Валюта != null)
            депозит.Валюта = depositDto.Валюта;
        if (depositDto.ПроцентнаяСтавка.HasValue)
            депозит.ПроцентнаяСтавка = depositDto.ПроцентнаяСтавка.Value;
        if (depositDto.ПроцентНалога.HasValue)
            депозит.ПроцентНалога = depositDto.ПроцентНалога.Value;
        if (depositDto.СпособВыплаты != null)
            депозит.СпособВыплаты = depositDto.СпособВыплаты;
        if (depositDto.МинимальныйСрок.HasValue)
            депозит.МинимальныйСрок = depositDto.МинимальныйСрок.Value;
        if (depositDto.ДатаЗакрытия.HasValue)
            депозит.ДатаЗакрытия = depositDto.ДатаЗакрытия.Value;
        if (depositDto.ДатаСледующейКапитализации.HasValue)
            депозит.ДатаСледующейКапитализации = depositDto.ДатаСледующейКапитализации.Value;
        if (depositDto.Статус != null && депозит.ДепозитныеСчета != null)
            депозит.ДепозитныеСчета.Статус = depositDto.Статус;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteDepositAsync(string номерДепозитногоДоговора)
    {
        var депозит = await _context.Депозитs.FindAsync(номерДепозитногоДоговора);
        if (депозит is null)
            throw new KeyNotFoundException($"Депозит с номером договора {номерДепозитногоДоговора} не найден");

        var депозитныйСчет = await _context.ДепозитныеСчетаs.FindAsync(номерДепозитногоДоговора);
        if (депозитныйСчет != null)
            _context.ДепозитныеСчетаs.Remove(депозитныйСчет);

        _context.Депозитs.Remove(депозит);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<DepositResponse>> SearchDepositsAsync(string searchTerm)
    {
        var депозиты = await _context.Депозитs
            .Include(d => d.ДепозитныеСчета)
            .AsNoTracking()
            .Where(d =>
                d.НомерДепозитногоДоговора.Contains(searchTerm) ||
                d.Название.Contains(searchTerm) ||
                d.ТипДепозита.Contains(searchTerm) ||
                d.Валюта.Contains(searchTerm))
            .ToListAsync();

        return депозиты.Select(ДепозитВОтвет);
    }

    public async Task<IEnumerable<DepositResponse>> GetDepositsByStatusAsync(string статус)
    {
        var депозиты = await _context.Депозитs
            .Include(d => d.ДепозитныеСчета)
            .AsNoTracking()
            .Where(d => d.ДепозитныеСчета != null && d.ДепозитныеСчета.Статус == статус)
            .ToListAsync();

        return депозиты.Select(ДепозитВОтвет);
    }

    public async Task<IEnumerable<DepositResponse>> GetDepositsByTypeAsync(string типДепозита)
    {
        var депозиты = await _context.Депозитs
            .Include(d => d.ДепозитныеСчета)
            .AsNoTracking()
            .Where(d => d.ТипДепозита == типДепозита)
            .ToListAsync();

        return депозиты.Select(ДепозитВОтвет);
    }

    public async Task<IEnumerable<DepositResponse>> GetDepositsByAccountAsync(string номерСчета)
    {
        var текущаяДата = DateOnly.FromDateTime(DateTime.Now);

        var депозитыПоСчету = await _context.ДепозитныеСчетаs
            .Where(ds => ds.НомерСчета == номерСчета)
            .Include(ds => ds.НомерДепозитногоДоговораNavigation)
            .Where(ds => ds.НомерДепозитногоДоговораNavigation != null &&
                        ds.НомерДепозитногоДоговораNavigation.ДатаОткрытия <= текущаяДата)
            .AsNoTracking()
            .ToListAsync();

        var результат = депозитыПоСчету
            .Select(ds => ДепозитВОтвет(ds.НомерДепозитногоДоговораNavigation));

        return результат;
    }

    private static DepositResponse ДепозитВОтвет(Депозит депозит)
    {
        return new DepositResponse(
            депозит.НомерДепозитногоДоговора,
            депозит.Название,
            депозит.ТипДепозита,
            депозит.СуммаДепозита,
            депозит.Валюта,
            депозит.ПроцентнаяСтавка,
            депозит.ПроцентНалога,
            депозит.ДатаОткрытия,
            депозит.ДатаЗакрытия,
            депозит.СпособВыплаты,
            депозит.МинимальныйСрок,
            депозит.ДатаСледующейКапитализации,
            депозит.ДепозитныеСчета != null ? new DepositAccountResponse(
                депозит.ДепозитныеСчета.НомерДепозитногоДоговора,
                депозит.ДепозитныеСчета.НомерСчета,
                депозит.ДепозитныеСчета.Статус
            ) : null
        );
    }
}