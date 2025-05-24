using Microsoft.EntityFrameworkCore;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Services;

public class AccountService : IAccountService
{
    private readonly NexusContext _context;

    public AccountService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AccountResponse>> GetAllAccountsAsync()
    {
        var счета = await _context.Счетs
            .AsNoTracking()
            .ToListAsync();

        return счета.Select(СчетВОтвет);
    }

    public async Task<AccountResponse> GetAccountByNumberAsync(string номерСчета)
    {
        var счет = await _context.Счетs
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.НомерСчета == номерСчета);

        if (счет is null)
            throw new KeyNotFoundException($"Счет с номером {номерСчета} не найден");

        return СчетВОтвет(счет);
    }

    public async Task<AccountResponse> CreateAccountAsync(AccountCreateRequest accountDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Проверяем существование клиента
            var клиент = await _context.Клиентs.FindAsync(accountDto.КлиентId);
            if (клиент == null)
            {
                throw new KeyNotFoundException($"Клиент с ID {accountDto.КлиентId} не найден");
            }

            var новыйСчет = new Счет
            {
                ТипСчета = accountDto.ТипСчета,
                НаименованиеСчета = accountDto.НаименованиеСчета,
                БикБанка = accountDto.БикБанка,
                КодФилиала = accountDto.КодФилиала,
                ДатаОткрытия = DateOnly.FromDateTime(DateTime.Now)
            };

            await _context.Счетs.AddAsync(новыйСчет);
            await _context.SaveChangesAsync();

            var счетКлиента = new СчетаКлиентов
            {
                НомерСчета = новыйСчет.НомерСчета,
                Клиент = accountDto.КлиентId,
                Статус = "Активен"
            };

            await _context.СчетаКлиентовs.AddAsync(счетКлиента);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return СчетВОтвет(новыйСчет);
        }
        catch (KeyNotFoundException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Ошибка при создании счета: {ex.Message}", ex);
        }
    }

    public async Task UpdateAccountAsync(string номерСчета, AccountUpdateRequest accountDto)
    {
        var счет = await _context.Счетs.FindAsync(номерСчета);

        if (счет is null)
            throw new KeyNotFoundException($"Счет с номером {номерСчета} не найден");

        if (accountDto.НаименованиеСчета != null)
            счет.НаименованиеСчета = accountDto.НаименованиеСчета;

        if (accountDto.БикБанка != null)
            счет.БикБанка = accountDto.БикБанка;

        if (accountDto.КодФилиала != null)
            счет.КодФилиала = accountDto.КодФилиала;

        if (accountDto.ДатаЗакрытия.HasValue)
            счет.ДатаЗакрытия = accountDto.ДатаЗакрытия.Value;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAccountAsync(string номерСчета)
    {
        var счет = await _context.Счетs.FindAsync(номерСчета);

        if (счет is null)
            throw new KeyNotFoundException($"Счет с номером {номерСчета} не найден");

        _context.Счетs.Remove(счет);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AccountResponse>> SearchAccountsAsync(string searchTerm)
    {
        var счета = await _context.Счетs
            .AsNoTracking()
            .Where(s =>
                s.НомерСчета.Contains(searchTerm) ||
                s.НаименованиеСчета.Contains(searchTerm) ||
                s.БикБанка.Contains(searchTerm) ||
                s.КодФилиала.Contains(searchTerm))
            .ToListAsync();

        return счета.Select(СчетВОтвет);
    }

    public async Task<IEnumerable<AccountResponse>> GetAccountsByTypeAsync(string типСчета)
    {
        var счета = await _context.Счетs
            .AsNoTracking()
            .Where(s => s.ТипСчета == типСчета)
            .ToListAsync();

        return счета.Select(СчетВОтвет);
    }

    private static AccountResponse СчетВОтвет(Счет счет)
    {
        return new AccountResponse(
            счет.НомерСчета,
            счет.ТипСчета,
            счет.НаименованиеСчета,
            счет.БикБанка,
            счет.КодФилиала,
            счет.ДатаОткрытия,
            счет.ДатаЗакрытия);
    }
}