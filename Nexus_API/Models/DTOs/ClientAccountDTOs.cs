namespace Nexus_API.Models.DTOs;

public record ClientAccountResponse(
    string НомерСчета,
    int Клиент,
    string Статус
);

public record ClientAccountCreateRequest(
    string НомерСчета,
    int Клиент,
    string Статус
);

public record ClientAccountUpdateRequest(
    int? Клиент,
    string? Статус
); 