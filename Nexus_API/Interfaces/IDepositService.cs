using Nexus_API.Models.DTOs;

namespace Nexus_API.Interfaces;

public interface IDepositService
{
    Task<IEnumerable<DepositResponse>> GetAllAsync();
    Task<DepositResponse?> GetByNumberAsync(string номерДоговора);
    Task<DepositResponse> CreateAsync(DepositCreateRequest request);
    Task UpdateAsync(string номерДоговора, DepositUpdateRequest request);
    Task DeleteAsync(string номерДоговора);
}