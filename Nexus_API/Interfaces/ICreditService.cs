using Nexus_API.Models.DTOs;

namespace Nexus_API.Interfaces;

public interface ICreditService
{
    Task<IEnumerable<CreditResponse>> GetAllCreditsAsync();
    Task<CreditResponse> GetCreditByNumberAsync(string номерДоговора);
    Task<CreditResponse> CreateCreditAsync(CreditCreateRequest creditDto);
    Task UpdateCreditAsync(string номерДоговора, CreditUpdateRequest creditDto);
    Task DeleteCreditAsync(string номерДоговора);
    Task<IEnumerable<CreditResponse>> SearchCreditsAsync(string searchTerm);
    Task<IEnumerable<CreditResponse>> GetCreditsByStatusAsync(string статус);
    Task<IEnumerable<CreditResponse>> GetCreditsByTypeAsync(int типКредита);
    Task<IEnumerable<PaymentScheduleResponse>> GetPaymentScheduleAsync(string номерДоговора);
    Task<IEnumerable<CreditResponse>> GetCreditsByAccountAsync(string номерСчета);
}