namespace Nexus_API.Services;

using Nexus_API.Models;
using Nexus_API.Models.DTOs;
using Nexus_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class CardService : ICardService
{
    private readonly NexusContext _context;

    public CardService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CardResponse>> GetAllCardsAsync()
    {
        return await _context.Картаs
            .Include(c => c.КарточныйПул)
            .Select(c => new CardResponse(
                c.НомерКарты,
                c.ДатаВыпуска,
                c.КодБезопасности,
                c.Лимит,
                c.ПлатежнаяСистема,
                c.СрокДействия,
                c.ТипКарты,
                c.КарточныйПул!.Статус,
                c.КарточныйПул.НомерСчета))
            .ToListAsync();
    }

    public async Task<CardResponse?> GetCardByIdAsync(string номерКарты)
    {
        var карта = await _context.Картаs
            .Include(c => c.КарточныйПул)
            .FirstOrDefaultAsync(c => c.НомерКарты == номерКарты);

        if (карта == null) return null;

        return new CardResponse(
            карта.НомерКарты,
            карта.ДатаВыпуска,
            карта.КодБезопасности,
            карта.Лимит,
            карта.ПлатежнаяСистема,
            карта.СрокДействия,
            карта.ТипКарты,
            карта.КарточныйПул?.Статус,
            карта.КарточныйПул?.НомерСчета);
    }

    public async Task<CardResponse> CreateCardAsync(CardCreateRequest request)
    {
        var accountExists = await _context.Счетs.AnyAsync(s => s.НомерСчета == request.НомерСчета);
        if (!accountExists)
            throw new ArgumentException("Указанный счет не существует");

        var карта = new Карта
        {
            НомерКарты = request.НомерКарты,
            ДатаВыпуска = DateOnly.FromDateTime(DateTime.Now),
            КодБезопасности = request.КодБезопасности,
            Лимит = request.Лимит,
            ПлатежнаяСистема = request.ПлатежнаяСистема,
            СрокДействия = request.СрокДействия,
            ТипКарты = request.ТипКарты
        };

        _context.Картаs.Add(карта);

        var карточныйПул = new КарточныйПул
        {
            НомерКарты = request.НомерКарты,
            НомерСчета = request.НомерСчета,
            Статус = request.Статус ?? "Активна"
        };

        _context.КарточныйПулs.Add(карточныйПул);
        await _context.SaveChangesAsync();

        return new CardResponse(
            карта.НомерКарты,
            карта.ДатаВыпуска,
            карта.КодБезопасности,
            карта.Лимит,
            карта.ПлатежнаяСистема,
            карта.СрокДействия,
            карта.ТипКарты,
            карточныйПул.Статус,
            карточныйПул.НомерСчета);
    }

    public async Task<CardResponse?> UpdateCardAsync(string номерКарты, CardUpdateRequest request)
    {
        var карта = await _context.Картаs
            .Include(c => c.КарточныйПул)
            .FirstOrDefaultAsync(c => c.НомерКарты == номерКарты);

        if (карта == null) return null;

        if (request.НомерКарты != null)
            карта.НомерКарты = request.НомерКарты;

        if (request.ДатаВыпуска.HasValue)
            карта.ДатаВыпуска = request.ДатаВыпуска.Value;

        if (request.КодБезопасности != null)
            карта.КодБезопасности = request.КодБезопасности;

        if (request.Лимит.HasValue)
            карта.Лимит = request.Лимит.Value;

        if (request.ПлатежнаяСистема != null)
            карта.ПлатежнаяСистема = request.ПлатежнаяСистема;

        if (request.СрокДействия != null)
            карта.СрокДействия = request.СрокДействия;

        if (request.ТипКарты != null)
            карта.ТипКарты = request.ТипКарты;

        if (request.Статус != null && карта.КарточныйПул != null)
            карта.КарточныйПул.Статус = request.Статус;

        if (request.НомерСчета != null && карта.КарточныйПул != null)
        {
            var accountExists = await _context.Счетs.AnyAsync(s => s.НомерСчета == request.НомерСчета);
            if (!accountExists)
                throw new ArgumentException("Указанный счет не существует");

            карта.КарточныйПул.НомерСчета = request.НомерСчета;
        }

        await _context.SaveChangesAsync();

        return new CardResponse(
            карта.НомерКарты,
            карта.ДатаВыпуска,
            карта.КодБезопасности,
            карта.Лимит,
            карта.ПлатежнаяСистема,
            карта.СрокДействия,
            карта.ТипКарты,
            карта.КарточныйПул?.Статус,
            карта.КарточныйПул?.НомерСчета);
    }

    public async Task<bool> DeleteCardAsync(string номерКарты)
    {
        var карта = await _context.Картаs
            .Include(c => c.КарточныйПул)
            .FirstOrDefaultAsync(c => c.НомерКарты == номерКарты);

        if (карта == null) return false;

        if (карта.КарточныйПул != null)
            _context.КарточныйПулs.Remove(карта.КарточныйПул);

        _context.Картаs.Remove(карта);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<CardResponse>> GetCardsByAccountAsync(string номерСчета)
    {
        return await _context.КарточныйПулs
            .Where(cp => cp.НомерСчета == номерСчета)
            .Include(cp => cp.НомерКартыNavigation)
            .Select(cp => new CardResponse(
                cp.НомерКарты,
                cp.НомерКартыNavigation.ДатаВыпуска,
                cp.НомерКартыNavigation.КодБезопасности,
                cp.НомерКартыNavigation.Лимит,
                cp.НомерКартыNavigation.ПлатежнаяСистема,
                cp.НомерКартыNavigation.СрокДействия,
                cp.НомерКартыNavigation.ТипКарты,
                cp.Статус,
                cp.НомерСчета))
            .ToListAsync();
    }

    public async Task<IEnumerable<CardResponse>> GetCardsByStatusAsync(string статус)
    {
        return await _context.КарточныйПулs
            .Where(cp => cp.Статус == статус)
            .Include(cp => cp.НомерКартыNavigation)
            .Select(cp => new CardResponse(
                cp.НомерКарты,
                cp.НомерКартыNavigation.ДатаВыпуска,
                cp.НомерКартыNavigation.КодБезопасности,
                cp.НомерКартыNavigation.Лимит,
                cp.НомерКартыNavigation.ПлатежнаяСистема,
                cp.НомерКартыNavigation.СрокДействия,
                cp.НомерКартыNavigation.ТипКарты,
                cp.Статус,
                cp.НомерСчета))
            .ToListAsync();
    }

    public async Task<IEnumerable<CardResponse>> GetExpiredCardsAsync()
    {
        var currentDate = DateTime.Now;
        var currentMonthYear = currentDate.ToString("MM/yy");

        return await _context.Картаs
            .Where(c => string.Compare(c.СрокДействия, currentMonthYear) < 0)
            .Include(c => c.КарточныйПул)
            .Select(c => new CardResponse(
                c.НомерКарты,
                c.ДатаВыпуска,
                c.КодБезопасности,
                c.Лимит,
                c.ПлатежнаяСистема,
                c.СрокДействия,
                c.ТипКарты,
                c.КарточныйПул!.Статус,
                c.КарточныйПул.НомерСчета))
            .ToListAsync();
    }
}