using Nexus_API.Models.DTOs;

namespace Nexus_API.Interfaces;

public interface ICardPoolService
{
    Task<IEnumerable<CardPoolResponse>> GetAllAsync();
    Task<CardPoolResponse?> GetByCardNumberAsync(string номерКарты);
    Task<CardPoolResponse> CreateAsync(CardPoolCreateRequest request);
    Task<CardPoolResponse?> UpdateAsync(string номерКарты, CardPoolUpdateRequest request);
    Task<bool> DeleteAsync(string номерКарты);
    Task<IEnumerable<CardPoolResponse>> GetByAccountAsync(string номерСчета);
    Task<IEnumerable<CardPoolResponse>> GetByStatusAsync(string статус);
} 