namespace Nexus_API.Models.DTOs;

public record CreditTypeResponse(
    int КодПродукта,
    bool Активный,
    decimal БазоваяСтавка,
    string Категория,
    decimal МаксимальнаяСумма,
    int МаксимальныйСрок,
    decimal МинимальнаяСумма,
    int МинимальныйСрок,
    string Название,
    bool НеобходимоСтрахование,
    string Описание);

public record CreditTypeCreateRequest(
    bool Активный,
    decimal БазоваяСтавка,
    string Категория,
    decimal МаксимальнаяСумма,
    int МаксимальныйСрок,
    decimal МинимальнаяСумма,
    int МинимальныйСрок,
    string Название,
    bool НеобходимоСтрахование,
    string Описание);

public record CreditTypeUpdateRequest(
    bool? Активный,
    decimal? БазоваяСтавка,
    string? Категория,
    decimal? МаксимальнаяСумма,
    int? МаксимальныйСрок,
    decimal? МинимальнаяСумма,
    int? МинимальныйСрок,
    string? Название,
    bool? НеобходимоСтрахование,
    string? Описание);