using Nexus_API.Models.DTOs;

namespace Nexus_API.Interfaces;

public interface ICreditTypeService
{
    Task<IEnumerable<CreditTypeResponse>> GetAllAsync();
    Task<CreditTypeResponse?> GetByIdAsync(int id);
    Task<CreditTypeResponse> CreateAsync(CreditTypeCreateRequest request);
    Task UpdateAsync(int id, CreditTypeUpdateRequest request);
    Task DeleteAsync(int id);
}