using Microsoft.EntityFrameworkCore;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Models.DTOs;
using System.Linq;

namespace Nexus_API.Services;

public class DepositService : IDepositService
{
    private readonly NexusContext _context;

    public DepositService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepositResponse>> GetAllAsync()
    {
        return await _context.Депозитs
            .Select(d => new DepositResponse(
                d.НомерДепозитногоДоговора,
                d.Валюта,
                d.ДатаОткрытия,
                d.ДатаЗакрытия,
                d.ДатаСледующейКапитализации,
                d.МинимальныйСрок,
                d.Название,
                d.ПроцентНалога,
                d.ПроцентнаяСтавка,
                d.СпособВыплаты,
                d.СуммаДепозита,
                d.ТипДепозита))
            .ToListAsync();
    }

    public async Task<DepositResponse?> GetByNumberAsync(string номерДоговора)
    {
        var депозит = await _context.Депозитs
            .FirstOrDefaultAsync(d => d.НомерДепозитногоДоговора == номерДоговора);

        if (депозит == null) return null;

        return new DepositResponse(
            депозит.НомерДепозитногоДоговора,
            депозит.Валюта,
            депозит.ДатаОткрытия,
            депозит.ДатаЗакрытия,
            депозит.ДатаСледующейКапитализации,
            депозит.МинимальныйСрок,
            депозит.Название,
            депозит.ПроцентНалога,
            депозит.ПроцентнаяСтавка,
            депозит.СпособВыплаты,
            депозит.СуммаДепозита,
            депозит.ТипДепозита);
    }

    public async Task<DepositResponse> CreateAsync(DepositCreateRequest request)
    {
        var депозит = new Депозит
        {
            НомерДепозитногоДоговора = Guid.NewGuid().ToString(),
            Валюта = request.Валюта,
            МинимальныйСрок = request.МинимальныйСрок,
            Название = request.Название,
            ПроцентНалога = request.ПроцентНалога,
            ПроцентнаяСтавка = request.ПроцентнаяСтавка,
            СпособВыплаты = request.СпособВыплаты,
            СуммаДепозита = request.СуммаДепозита,
            ТипДепозита = request.ТипДепозита
        };

        _context.Депозитs.Add(депозит);
        await _context.SaveChangesAsync();

        return new DepositResponse(
            депозит.НомерДепозитногоДоговора,
            депозит.Валюта,
            депозит.ДатаОткрытия,
            депозит.ДатаЗакрытия,
            депозит.ДатаСледующейКапитализации,
            депозит.МинимальныйСрок,
            депозит.Название,
            депозит.ПроцентНалога,
            депозит.ПроцентнаяСтавка,
            депозит.СпособВыплаты,
            депозит.СуммаДепозита,
            депозит.ТипДепозита);
    }

    public async Task UpdateAsync(string номерДоговора, DepositUpdateRequest request)
    {
        var депозит = await _context.Депозитs
            .FirstOrDefaultAsync(d => d.НомерДепозитногоДоговора == номерДоговора);

        if (депозит == null) return;

        if (request.ДатаЗакрытия != null) депозит.ДатаЗакрытия = request.ДатаЗакрытия;
        if (request.ДатаСледующейКапитализации != null) депозит.ДатаСледующейКапитализации = request.ДатаСледующейКапитализации;
        if (request.СпособВыплаты != null) депозит.СпособВыплаты = request.СпособВыплаты;
        if (request.ПроцентнаяСтавка != null) депозит.ПроцентнаяСтавка = request.ПроцентнаяСтавка.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string номерДоговора)
    {
        var депозит = await _context.Депозитs
            .FirstOrDefaultAsync(d => d.НомерДепозитногоДоговора == номерДоговора);

        if (депозит == null) return;

        _context.Депозитs.Remove(депозит);
        await _context.SaveChangesAsync();
    }
}