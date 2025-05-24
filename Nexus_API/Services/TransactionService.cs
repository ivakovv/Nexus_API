using Microsoft.EntityFrameworkCore;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Services;

public class TransactionService : ITransactionService
{
    private readonly NexusContext _context;

    public TransactionService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync()
    {
        var истории = await _context.ИсторияТранзакцийs
            .Include(t => t.КодТранзакцииNavigation)
            .AsNoTracking()
            .ToListAsync();
        return истории.Select(ИсторияВОтвет);
    }

    public async Task<TransactionResponse> GetTransactionByCodeAsync(string кодТранзакции)
    {
        var история = await _context.ИсторияТранзакцийs
            .Include(t => t.КодТранзакцииNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.КодТранзакции == кодТранзакции);
        if (история is null)
            throw new KeyNotFoundException($"Транзакция с кодом {кодТранзакции} не найдена");
        return ИсторияВОтвет(история);
    }

    public async Task<TransactionResponse> CreateTransactionAsync(TransactionCreateRequest transactionDto)
    {
        var кодТранзакции = GenerateTransactionCode();
        var транзакция = new Транзакция
        {
            КодТранзакции = кодТранзакции,
            ТипТранзакции = transactionDto.ТипТранзакции,
            Направление = transactionDto.Направление
        };
        await _context.Транзакцияs.AddAsync(транзакция);
        await _context.SaveChangesAsync();

        var история = new ИсторияТранзакций
        {
            КодТранзакции = кодТранзакции,
            СчетОтправителя = transactionDto.СчетОтправителя,
            СчетПолучателя = transactionDto.СчетПолучателя,
            Сумма = transactionDto.Сумма,
            ВнешнийСчетОтправителя = transactionDto.ВнешнийСчетОтправителя,
            БикБанкаОтправителя = transactionDto.БикБанкаОтправителя,
            ВнешнийСчетПолучателя = transactionDto.ВнешнийСчетПолучателя,
            БикБанкаПолучателя = transactionDto.БикБанкаПолучателя,
            Комиссия = transactionDto.Комиссия,
            ДатаСоздания = DateTime.Now,
            Статус = "Создана"
        };
        await _context.ИсторияТранзакцийs.AddAsync(история);
        await _context.SaveChangesAsync();

        return await GetTransactionByCodeAsync(кодТранзакции);
    }

    public async Task UpdateTransactionAsync(string кодТранзакции, TransactionUpdateRequest transactionDto)
    {
        var история = await _context.ИсторияТранзакцийs.FindAsync(кодТранзакции);
        if (история is null)
            throw new KeyNotFoundException($"Транзакция с кодом {кодТранзакции} не найдена");
        if (transactionDto.Статус != null)
            история.Статус = transactionDto.Статус;
        if (transactionDto.ДатаВыполнения.HasValue)
            история.ДатаВыполнения = transactionDto.ДатаВыполнения.Value;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTransactionAsync(string кодТранзакции)
    {
        var история = await _context.ИсторияТранзакцийs.FindAsync(кодТранзакции);
        if (история is null)
            throw new KeyNotFoundException($"Транзакция с кодом {кодТранзакции} не найдена");
        var транзакция = await _context.Транзакцияs.FindAsync(кодТранзакции);
        if (транзакция != null)
            _context.Транзакцияs.Remove(транзакция);
        _context.ИсторияТранзакцийs.Remove(история);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TransactionResponse>> SearchTransactionsAsync(string searchTerm)
    {
        var истории = await _context.ИсторияТранзакцийs
            .Include(t => t.КодТранзакцииNavigation)
            .AsNoTracking()
            .Where(t =>
                t.КодТранзакции.Contains(searchTerm) ||
                t.СчетОтправителя.Contains(searchTerm) ||
                t.СчетПолучателя.Contains(searchTerm) ||
                t.Статус.Contains(searchTerm))
            .ToListAsync();
        return истории.Select(ИсторияВОтвет);
    }

    public async Task<IEnumerable<TransactionResponse>> GetTransactionsByStatusAsync(string статус)
    {
        var истории = await _context.ИсторияТранзакцийs
            .Include(t => t.КодТранзакцииNavigation)
            .AsNoTracking()
            .Where(t => t.Статус == статус)
            .ToListAsync();
        return истории.Select(ИсторияВОтвет);
    }

    public async Task<IEnumerable<TransactionResponse>> GetTransactionsByTypeAsync(string типТранзакции)
    {
        var истории = await _context.ИсторияТранзакцийs
            .Include(t => t.КодТранзакцииNavigation)
            .AsNoTracking()
            .Where(t => t.КодТранзакцииNavigation.ТипТранзакции == типТранзакции)
            .ToListAsync();
        return истории.Select(ИсторияВОтвет);
    }

    private static TransactionResponse ИсторияВОтвет(ИсторияТранзакций история)
    {
        return new TransactionResponse(
            история.КодТранзакции,
            история.КодТранзакцииNavigation?.ТипТранзакции ?? string.Empty,
            история.КодТранзакцииNavigation?.Направление ?? string.Empty,
            история.СчетОтправителя,
            история.СчетПолучателя,
            история.Сумма,
            история.ВнешнийСчетОтправителя,
            история.БикБанкаОтправителя,
            история.ВнешнийСчетПолучателя,
            история.БикБанкаПолучателя,
            история.Комиссия,
            история.ДатаСоздания,
            история.ДатаВыполнения,
            история.Статус
        );
    }

    private static string GenerateTransactionCode()
    {
        return $"TR{DateTime.Now:yyyyMMdd}{Random.Shared.Next(100000, 999999)}";
    }
} 