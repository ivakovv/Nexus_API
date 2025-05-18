using Microsoft.AspNetCore.Mvc;
using Nexus_API.Interfaces;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepositsController : ControllerBase
{
    private readonly IDepositService _service;

    public DepositsController(IDepositService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepositResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{number}")]
    public async Task<ActionResult<DepositResponse>> GetByNumber(string number)
    {
        var deposit = await _service.GetByNumberAsync(number);
        if (deposit == null) return NotFound();
        return Ok(deposit);
    }

    [HttpPost]
    public async Task<ActionResult<DepositResponse>> Create(DepositCreateRequest request)
    {
        var deposit = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetByNumber), new { number = deposit.НомерДепозитногоДоговора }, deposit);
    }

    [HttpPut("{number}")]
    public async Task<IActionResult> Update(string number, DepositUpdateRequest request)
    {
        await _service.UpdateAsync(number, request);
        return NoContent();
    }

    [HttpDelete("{number}")]
    public async Task<IActionResult> Delete(string number)
    {
        await _service.DeleteAsync(number);
        return NoContent();
    }
}