using Nexus_API.Models.DTOs;

namespace Nexus_API.Interfaces;

public interface IClientAccountService
{
    Task<IEnumerable<ClientAccountResponse>> GetAllAsync();
    Task<ClientAccountResponse?> GetByAccountNumberAsync(string номерСчета);
    Task<ClientAccountResponse> CreateAsync(ClientAccountCreateRequest request);
    Task<ClientAccountResponse?> UpdateAsync(string номерСчета, ClientAccountUpdateRequest request);
    Task<bool> DeleteAsync(string номерСчета);
    Task<IEnumerable<ClientAccountResponse>> GetByClientAsync(int клиент);
    Task<IEnumerable<ClientAccountResponse>> GetByStatusAsync(string статус);
} 