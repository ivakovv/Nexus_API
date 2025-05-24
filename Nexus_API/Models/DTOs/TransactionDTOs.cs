namespace Nexus_API.Models.DTOs;

public record TransactionResponse(
    string КодТранзакции,
    string ТипТранзакции,
    string Направление,
    string? СчетОтправителя,
    string? СчетПолучателя,
    decimal Сумма,
    string? ВнешнийСчетОтправителя,
    string? БикБанкаОтправителя,
    string? ВнешнийСчетПолучателя,
    string? БикБанкаПолучателя,
    decimal Комиссия,
    DateTime ДатаСоздания,
    DateTime? ДатаВыполнения,
    string Статус
);

public record TransactionCreateRequest(
    string ТипТранзакции,
    string Направление,
    string? СчетОтправителя,
    string? СчетПолучателя,
    decimal Сумма,
    string? ВнешнийСчетОтправителя,
    string? БикБанкаОтправителя,
    string? ВнешнийСчетПолучателя,
    string? БикБанкаПолучателя,
    decimal Комиссия
);

public record TransactionUpdateRequest(
    string? Статус,
    DateTime? ДатаВыполнения
); 