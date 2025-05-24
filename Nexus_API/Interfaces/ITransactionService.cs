using Nexus_API.Models.DTOs;

namespace Nexus_API.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionResponse>> GetAllTransactionsAsync();
    Task<TransactionResponse> GetTransactionByCodeAsync(string кодТранзакции);
    Task<TransactionResponse> CreateTransactionAsync(TransactionCreateRequest transactionDto);
    Task UpdateTransactionAsync(string кодТранзакции, TransactionUpdateRequest transactionDto);
    Task DeleteTransactionAsync(string кодТранзакции);
    Task<IEnumerable<TransactionResponse>> SearchTransactionsAsync(string searchTerm);
    Task<IEnumerable<TransactionResponse>> GetTransactionsByStatusAsync(string статус);
    Task<IEnumerable<TransactionResponse>> GetTransactionsByTypeAsync(string типТранзакции);
} 