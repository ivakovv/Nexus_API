﻿namespace Nexus_API.Models.DTOs;

public record ClientResponse(
    int Id,
    string Фамилия,
    string Имя,
    string? Отчество,
    string СерияПаспорта,
    string НомерПаспорта,
    string СтатусКлиента,
    string НомерТелефона,
    string Email,
    string АдресРегистрации,
    string АдресПроживания,
    DateOnly ДатаРождения,
    string Пол,
    string? Снилс,
    string? Инн,
    string? Работодатель,
    string? ИсточникДохода,
    decimal? УровеньДохода,
    string? СемейноеПоложение,
    string? МестоРождения,
    string? КемВыданПаспорт,
    DateOnly ДатаВыдачиПаспорта,
    string? ДополнительныеСведения);

public record ClientCreateRequest(
    string Фамилия,
    string Имя,
    string? Отчество,
    string СерияПаспорта,
    string НомерПаспорта,
    string НомерТелефона,
    string Email,
    string АдресРегистрации,
    string АдресПроживания,
    DateOnly ДатаРождения,
    string Пол,
    string? Снилс,
    string? Инн,
    string? Работодатель,
    string? ИсточникДохода,
    decimal? УровеньДохода,
    string? СемейноеПоложение,
    string? МестоРождения,
    string? КемВыданПаспорт,
    DateOnly ДатаВыдачиПаспорта,
    string? ДополнительныеСведения);

public record ClientUpdateRequest(
    string? СерияПаспорта,
    string? НомерПаспорта,
    string? Фамилия,
    string? Имя,
    string? Отчество,
    string? Пол,
    DateOnly? ДатаВыдачиПаспорта,
    string? КемВыданПаспорт,
    string? МестоРождения,
    DateOnly? ДатаРождения,
    string? АдресРегистрации,
    string? АдресПроживания,
    string? СемейноеПоложение,
    string? НомерТелефона,
    string? Email,
    string? Инн,
    string? Снилс,
    string? ИсточникДохода,
    string? Работодатель,
    decimal? УровеньДохода,
    string? СтатусКлиента,
    DateOnly? ДатаРегистрацииВБанке,
    string? ДополнительныеСведения);


