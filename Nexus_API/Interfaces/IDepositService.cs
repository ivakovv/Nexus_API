using Nexus_API.Models.DTOs;

namespace Nexus_API.Interfaces;

public interface IDepositService
{
    Task<IEnumerable<DepositResponse>> GetAllDepositsAsync();
    Task<DepositResponse> GetDepositByNumberAsync(string номерДепозитногоДоговора);
    Task<DepositResponse> CreateDepositAsync(DepositCreateRequest depositDto);
    Task UpdateDepositAsync(string номерДепозитногоДоговора, DepositUpdateRequest depositDto);
    Task DeleteDepositAsync(string номерДепозитногоДоговора);
    Task<IEnumerable<DepositResponse>> SearchDepositsAsync(string searchTerm);
    Task<IEnumerable<DepositResponse>> GetDepositsByStatusAsync(string статус);
    Task<IEnumerable<DepositResponse>> GetDepositsByTypeAsync(string типДепозита);
    Task<IEnumerable<DepositResponse>> GetDepositsByAccountAsync(string номерСчета);
}