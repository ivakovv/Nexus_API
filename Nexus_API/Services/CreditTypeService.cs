using Microsoft.EntityFrameworkCore;
using Nexus_API.Interfaces;
using Nexus_API.Models;
using Nexus_API.Models.DTOs;
using System.Linq;

namespace Nexus_API.Services;

public class CreditTypeService : ICreditTypeService
{
    private readonly NexusContext _context;

    public CreditTypeService(NexusContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CreditTypeResponse>> GetAllAsync()
    {
        return await _context.ВидКредитаs
            .Select(v => new CreditTypeResponse(
                v.КодПродукта,
                v.Активный,
                v.БазоваяСтавка,
                v.Категория,
                (decimal)v.МаксимальнаяСумма,
                v.МаксимальныйСрок,
                (decimal)v.МинимальнаяСумма,
                v.МинимальныйСрок,
                v.Название,
                v.НеобходимоСтрахование,
                v.Описание))
            .ToListAsync();
    }

    public async Task<CreditTypeResponse?> GetByIdAsync(int id)
    {
        var видКредита = await _context.ВидКредитаs
            .FirstOrDefaultAsync(v => v.КодПродукта == id);

        if (видКредита == null) return null;

        return new CreditTypeResponse(
            видКредита.КодПродукта,
            видКредита.Активный,
            видКредита.БазоваяСтавка,
            видКредита.Категория,
            (decimal)видКредита.МаксимальнаяСумма,
            видКредита.МаксимальныйСрок,
            (decimal)видКредита.МинимальнаяСумма,
            видКредита.МинимальныйСрок,
            видКредита.Название,
            видКредита.НеобходимоСтрахование,
            видКредита.Описание);
    }

    public async Task<CreditTypeResponse> CreateAsync(CreditTypeCreateRequest request)
    {
        var видКредита = new ВидКредита
        {
            Активный = request.Активный,
            БазоваяСтавка = request.БазоваяСтавка,
            Категория = request.Категория,
            МаксимальнаяСумма = request.МаксимальнаяСумма,
            МаксимальныйСрок = request.МаксимальныйСрок,
            МинимальнаяСумма = request.МинимальнаяСумма,
            МинимальныйСрок = request.МинимальныйСрок,
            Название = request.Название,
            НеобходимоСтрахование = request.НеобходимоСтрахование,
            Описание = request.Описание
        };

        _context.ВидКредитаs.Add(видКредита);
        await _context.SaveChangesAsync();

        return new CreditTypeResponse(
            видКредита.КодПродукта,
            видКредита.Активный,
            видКредита.БазоваяСтавка,
            видКредита.Категория,
            (decimal)видКредита.МаксимальнаяСумма,
            видКредита.МаксимальныйСрок,
            (decimal)видКредита.МинимальнаяСумма,
            видКредита.МинимальныйСрок,
            видКредита.Название,
            видКредита.НеобходимоСтрахование,
            видКредита.Описание);
    }

    public async Task UpdateAsync(int id, CreditTypeUpdateRequest request)
    {
        var видКредита = await _context.ВидКредитаs
            .FirstOrDefaultAsync(v => v.КодПродукта == id);

        if (видКредита == null) return;

        if (request.Активный != null) видКредита.Активный = request.Активный.Value;
        if (request.БазоваяСтавка != null) видКредита.БазоваяСтавка = request.БазоваяСтавка.Value;
        if (request.Категория != null) видКредита.Категория = request.Категория;
        if (request.МаксимальнаяСумма != null) видКредита.МаксимальнаяСумма = request.МаксимальнаяСумма.Value;
        if (request.МаксимальныйСрок != null) видКредита.МаксимальныйСрок = request.МаксимальныйСрок.Value;
        if (request.МинимальнаяСумма != null) видКредита.МинимальнаяСумма = request.МинимальнаяСумма.Value;
        if (request.МинимальныйСрок != null) видКредита.МинимальныйСрок = request.МинимальныйСрок.Value;
        if (request.Название != null) видКредита.Название = request.Название;
        if (request.НеобходимоСтрахование != null) видКредита.НеобходимоСтрахование = request.НеобходимоСтрахование.Value;
        if (request.Описание != null) видКредита.Описание = request.Описание;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var видКредита = await _context.ВидКредитаs
            .FirstOrDefaultAsync(v => v.КодПродукта == id);

        if (видКредита == null) return;

        _context.ВидКредитаs.Remove(видКредита);
        await _context.SaveChangesAsync();
    }
}