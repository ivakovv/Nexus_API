using Microsoft.EntityFrameworkCore;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Services;

public class ClientService : IClientService
{
    private readonly NexusContext _context;

    public ClientService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientResponse>> GetAllClientsAsync()
    {
        var клиенты = await _context.Клиентs
            .AsNoTracking()
            .ToListAsync();

        return клиенты.Select(КлиентВОтвет);
    }

    public async Task<ClientResponse> GetClientByIdAsync(int id)
    {
        var клиент = await _context.Клиентs
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.Id == id);

        if (клиент is null)
            throw new KeyNotFoundException($"Клиент с ID {id} не найден");

        return КлиентВОтвет(клиент);
    }

    public async Task<ClientResponse> CreateClientAsync(ClientCreateRequest clientDto)
    {
        var exists = await _context.Клиентs.AnyAsync(k =>
            k.СерияПаспорта == clientDto.СерияПаспорта &&
            k.НомерПаспорта == clientDto.НомерПаспорта);

        if (exists)
            throw new InvalidOperationException("Клиент с такими паспортными данными уже существует");

 
        if (!string.IsNullOrEmpty(clientDto.Инн))
        {
            var innExists = await _context.Клиентs.AnyAsync(k => k.Инн == clientDto.Инн);
            if (innExists)
                throw new InvalidOperationException("Клиент с таким ИНН уже существует");
        }

  
        if (!string.IsNullOrEmpty(clientDto.Снилс))
        {
            var snilsExists = await _context.Клиентs.AnyAsync(k => k.Снилс == clientDto.Снилс);
            if (snilsExists)
                throw new InvalidOperationException("Клиент с таким СНИЛС уже существует");
        }

        var новыйКлиент = new Клиент
        {
            Фамилия = clientDto.Фамилия,
            Имя = clientDto.Имя,
            Отчество = clientDto.Отчество,
            СерияПаспорта = clientDto.СерияПаспорта,
            НомерПаспорта = clientDto.НомерПаспорта,
            НомерТелефона = clientDto.НомерТелефона,
            Email = clientDto.Email,
            АдресРегистрации = clientDto.АдресРегистрации,
            АдресПроживания = clientDto.АдресПроживания,
            ДатаРождения = clientDto.ДатаРождения,
            Пол = clientDto.Пол,
            Снилс = clientDto.Снилс,
            Инн = clientDto.Инн,
            Работодатель = clientDto.Работодатель,
            ИсточникДохода = clientDto.ИсточникДохода,
            УровеньДохода = clientDto.УровеньДохода,
            СемейноеПоложение = clientDto.СемейноеПоложение,
            МестоРождения = clientDto.МестоРождения,
            КемВыданПаспорт = clientDto.КемВыданПаспорт,
            ДатаВыдачиПаспорта = clientDto.ДатаВыдачиПаспорта,
            ДополнительныеСведения = clientDto.ДополнительныеСведения,
            СтатусКлиента = "Активен"
        };

        await _context.Клиентs.AddAsync(новыйКлиент);
        await _context.SaveChangesAsync();

        return КлиентВОтвет(новыйКлиент);
    }

    public async Task UpdateClientAsync(int id, ClientUpdateRequest clientDto)
    {
        var клиент = await _context.Клиентs.FindAsync(id);

        if (клиент is null)
            throw new KeyNotFoundException($"Клиент с ID {id} не найден");

        if (clientDto.Фамилия != null)
            клиент.Фамилия = clientDto.Фамилия;

        if (clientDto.Имя != null)
            клиент.Имя = clientDto.Имя;

        if (clientDto.Отчество != null)
            клиент.Отчество = clientDto.Отчество;

        if (clientDto.НомерТелефона != null)
            клиент.НомерТелефона = clientDto.НомерТелефона;

        if (clientDto.Email != null)
            клиент.Email = clientDto.Email;

        if (clientDto.АдресПроживания != null)
            клиент.АдресПроживания = clientDto.АдресПроживания;

        if (clientDto.СтатусКлиента != null)
            клиент.СтатусКлиента = clientDto.СтатусКлиента;

        if (clientDto.Работодатель != null)
            клиент.Работодатель = clientDto.Работодатель;

        if (clientDto.ИсточникДохода != null)
            клиент.ИсточникДохода = clientDto.ИсточникДохода;

        if (clientDto.УровеньДохода.HasValue)
            клиент.УровеньДохода = clientDto.УровеньДохода.Value;

        if (clientDto.СемейноеПоложение != null)
            клиент.СемейноеПоложение = clientDto.СемейноеПоложение;

        if (clientDto.ДополнительныеСведения != null)
            клиент.ДополнительныеСведения = clientDto.ДополнительныеСведения;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteClientAsync(int id)
    {
        var клиент = await _context.Клиентs.FindAsync(id);

        if (клиент is null)
            throw new KeyNotFoundException($"Клиент с ID {id} не найден");

        _context.Клиентs.Remove(клиент);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ClientResponse>> SearchClientsAsync(string searchTerm)
    {
        var клиенты = await _context.Клиентs
            .AsNoTracking()
            .Where(k =>
                k.Фамилия.Contains(searchTerm) ||
                k.Имя.Contains(searchTerm) ||
                k.Отчество.Contains(searchTerm) ||
                k.НомерТелефона.Contains(searchTerm) ||
                k.СерияПаспорта.Contains(searchTerm) ||
                k.НомерПаспорта.Contains(searchTerm) ||
                (k.Инн != null && k.Инн.Contains(searchTerm)) ||
                (k.Снилс != null && k.Снилс.Contains(searchTerm)))
            .ToListAsync();

        return клиенты.Select(КлиентВОтвет);
    }

    public async Task<IEnumerable<ClientResponse>> GetClientsByStatusAsync(string status)
    {
        var клиенты = await _context.Клиентs
            .AsNoTracking()
            .Where(k => k.СтатусКлиента == status)
            .ToListAsync();

        return клиенты.Select(КлиентВОтвет);
    }

    private static ClientResponse КлиентВОтвет(Клиент клиент)
    {
        return new ClientResponse(
            клиент.Id,
            клиент.Фамилия,
            клиент.Имя,
            клиент.Отчество,
            клиент.СерияПаспорта,
            клиент.НомерПаспорта,
            клиент.СтатусКлиента,
            клиент.НомерТелефона,
            клиент.Email,
            клиент.АдресРегистрации,
            клиент.АдресПроживания,
            клиент.ДатаРождения,
            клиент.Пол,
            клиент.Снилс,
            клиент.Инн,
            клиент.Работодатель,
            клиент.ИсточникДохода,
            клиент.УровеньДохода,
            клиент.СемейноеПоложение,
            клиент.МестоРождения,
            клиент.КемВыданПаспорт,
            клиент.ДатаВыдачиПаспорта,
            клиент.ДополнительныеСведения);
    }
}