using Microsoft.EntityFrameworkCore;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Services;

public class ClientAccountService : IClientAccountService
{
    private readonly NexusContext _context;

    public ClientAccountService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientAccountResponse>> GetAllAsync()
    {
        var accounts = await _context.СчетаКлиентовs.AsNoTracking().ToListAsync();
        return accounts.Select(ToResponse);
    }

    public async Task<ClientAccountResponse?> GetByAccountNumberAsync(string номерСчета)
    {
        var acc = await _context.СчетаКлиентовs.AsNoTracking().FirstOrDefaultAsync(x => x.НомерСчета == номерСчета);
        return acc == null ? null : ToResponse(acc);
    }

    public async Task<ClientAccountResponse> CreateAsync(ClientAccountCreateRequest request)
    {
        var acc = new СчетаКлиентов
        {
            НомерСчета = request.НомерСчета,
            Клиент = request.Клиент,
            Статус = request.Статус
        };
        await _context.СчетаКлиентовs.AddAsync(acc);
        await _context.SaveChangesAsync();
        return ToResponse(acc);
    }

    public async Task<ClientAccountResponse?> UpdateAsync(string номерСчета, ClientAccountUpdateRequest request)
    {
        var acc = await _context.СчетаКлиентовs.FirstOrDefaultAsync(x => x.НомерСчета == номерСчета);
        if (acc == null) return null;
        if (request.Клиент.HasValue) acc.Клиент = request.Клиент.Value;
        if (request.Статус != null) acc.Статус = request.Статус;
        await _context.SaveChangesAsync();
        return ToResponse(acc);
    }

    public async Task<bool> DeleteAsync(string номерСчета)
    {
        var acc = await _context.СчетаКлиентовs.FirstOrDefaultAsync(x => x.НомерСчета == номерСчета);
        if (acc == null) return false;
        _context.СчетаКлиентовs.Remove(acc);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ClientAccountResponse>> GetByClientAsync(int клиент)
    {
        var accs = await _context.СчетаКлиентовs.AsNoTracking().Where(x => x.Клиент == клиент).ToListAsync();
        return accs.Select(ToResponse);
    }

    public async Task<IEnumerable<ClientAccountResponse>> GetByStatusAsync(string статус)
    {
        var accs = await _context.СчетаКлиентовs.AsNoTracking().Where(x => x.Статус == статус).ToListAsync();
        return accs.Select(ToResponse);
    }

    private static ClientAccountResponse ToResponse(СчетаКлиентов acc)
    {
        return new ClientAccountResponse(acc.НомерСчета, acc.Клиент, acc.Статус);
    }
} 