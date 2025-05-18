using Nexus_API.Models.DTOs;

namespace Nexus_API.Interfaces;

public interface IClientService
{
    Task<IEnumerable<ClientResponse>> GetAllClientsAsync();
    Task<ClientResponse> GetClientByIdAsync(int id);
    Task<ClientResponse> CreateClientAsync(ClientCreateRequest clientDto);
    Task UpdateClientAsync(int id, ClientUpdateRequest clientDto);
    Task DeleteClientAsync(int id);
    Task<IEnumerable<ClientResponse>> SearchClientsAsync(string searchTerm);
    Task<IEnumerable<ClientResponse>> GetClientsByStatusAsync(string status);
}