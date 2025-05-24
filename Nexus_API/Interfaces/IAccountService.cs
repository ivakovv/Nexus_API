using Nexus_API.Models.DTOs;

namespace Nexus_API.Interfaces;

public interface IAccountService
{
    Task<IEnumerable<AccountResponse>> GetAllAccountsAsync();
    Task<AccountResponse> GetAccountByNumberAsync(string номерСчета);
    Task<AccountResponse> CreateAccountAsync(AccountCreateRequest accountDto);
    Task UpdateAccountAsync(string номерСчета, AccountUpdateRequest accountDto);
    Task DeleteAccountAsync(string номерСчета);
    Task<IEnumerable<AccountResponse>> SearchAccountsAsync(string searchTerm);
    Task<IEnumerable<AccountResponse>> GetAccountsByTypeAsync(string типСчета);
} 