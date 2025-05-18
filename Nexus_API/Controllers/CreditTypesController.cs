using Microsoft.AspNetCore.Mvc;
using Nexus_API.Interfaces;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditTypesController : ControllerBase
{
    private readonly ICreditTypeService _service;

    public CreditTypesController(ICreditTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CreditTypeResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CreditTypeResponse>> GetById(int id)
    {
        var creditType = await _service.GetByIdAsync(id);
        if (creditType == null) return NotFound();
        return Ok(creditType);
    }

    [HttpPost]
    public async Task<ActionResult<CreditTypeResponse>> Create(CreditTypeCreateRequest request)
    {
        var creditType = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = creditType.КодПродукта }, creditType);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreditTypeUpdateRequest request)
    {
        await _service.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}