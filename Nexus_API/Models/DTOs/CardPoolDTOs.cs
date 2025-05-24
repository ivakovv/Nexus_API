namespace Nexus_API.Models.DTOs;

public record CardPoolResponse(
    string НомерКарты,
    string НомерСчета,
    string Статус
);

public record CardPoolCreateRequest(
    string НомерКарты,
    string НомерСчета,
    string Статус
);

public record CardPoolUpdateRequest(
    string? НомерСчета,
    string? Статус
); 