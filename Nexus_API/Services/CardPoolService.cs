using Microsoft.EntityFrameworkCore;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Services;

public class CardPoolService : ICardPoolService
{
    private readonly NexusContext _context;

    public CardPoolService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CardPoolResponse>> GetAllAsync()
    {
        var cardPools = await _context.КарточныйПулs.AsNoTracking().ToListAsync();
        return cardPools.Select(ToResponse);
    }

    public async Task<CardPoolResponse?> GetByCardNumberAsync(string номерКарты)
    {
        var pool = await _context.КарточныйПулs.AsNoTracking().FirstOrDefaultAsync(x => x.НомерКарты == номерКарты);
        return pool == null ? null : ToResponse(pool);
    }

    public async Task<CardPoolResponse> CreateAsync(CardPoolCreateRequest request)
    {
        var pool = new КарточныйПул
        {
            НомерКарты = request.НомерКарты,
            НомерСчета = request.НомерСчета,
            Статус = request.Статус
        };
        await _context.КарточныйПулs.AddAsync(pool);
        await _context.SaveChangesAsync();
        return ToResponse(pool);
    }

    public async Task<CardPoolResponse?> UpdateAsync(string номерКарты, CardPoolUpdateRequest request)
    {
        var pool = await _context.КарточныйПулs.FirstOrDefaultAsync(x => x.НомерКарты == номерКарты);
        if (pool == null) return null;
        if (request.НомерСчета != null) pool.НомерСчета = request.НомерСчета;
        if (request.Статус != null) pool.Статус = request.Статус;
        await _context.SaveChangesAsync();
        return ToResponse(pool);
    }

    public async Task<bool> DeleteAsync(string номерКарты)
    {
        var pool = await _context.КарточныйПулs.FirstOrDefaultAsync(x => x.НомерКарты == номерКарты);
        if (pool == null) return false;
        _context.КарточныйПулs.Remove(pool);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CardPoolResponse>> GetByAccountAsync(string номерСчета)
    {
        var pools = await _context.КарточныйПулs.AsNoTracking().Where(x => x.НомерСчета == номерСчета).ToListAsync();
        return pools.Select(ToResponse);
    }

    public async Task<IEnumerable<CardPoolResponse>> GetByStatusAsync(string статус)
    {
        var pools = await _context.КарточныйПулs.AsNoTracking().Where(x => x.Статус == статус).ToListAsync();
        return pools.Select(ToResponse);
    }

    private static CardPoolResponse ToResponse(КарточныйПул pool)
    {
        return new CardPoolResponse(pool.НомерКарты, pool.НомерСчета, pool.Статус);
    }
} 