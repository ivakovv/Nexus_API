namespace Nexus_API.Interfaces;

using Nexus_API.Models.DTOs;

public interface ICardService
{
    Task<IEnumerable<CardResponse>> GetAllCardsAsync();
    Task<CardResponse?> GetCardByIdAsync(string номерКарты);
    Task<CardResponse> CreateCardAsync(CardCreateRequest cardDto);
    Task<CardResponse?> UpdateCardAsync(string номерКарты, CardUpdateRequest cardDto);
    Task<bool> DeleteCardAsync(string номерКарты);
    Task<IEnumerable<CardResponse>> GetCardsByAccountAsync(string номерСчета);
    Task<IEnumerable<CardResponse>> GetCardsByStatusAsync(string статус);
}