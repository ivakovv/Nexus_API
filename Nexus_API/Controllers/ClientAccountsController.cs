using Microsoft.AspNetCore.Mvc;
using Nexus_API.Interfaces;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ClientAccountsController : ControllerBase
{
    private readonly IClientAccountService _service;

    public ClientAccountsController(IClientAccountService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientAccountResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{номерСчета}")]
    public async Task<ActionResult<ClientAccountResponse>> GetByAccountNumber(string номерСчета)
    {
        var acc = await _service.GetByAccountNumberAsync(номерСчета);
        return acc == null ? NotFound() : Ok(acc);
    }

    [HttpPost]
    public async Task<ActionResult<ClientAccountResponse>> Create([FromBody] ClientAccountCreateRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetByAccountNumber), new { номерСчета = created.НомерСчета }, created);
    }

    [HttpPut("{номерСчета}")]
    public async Task<ActionResult<ClientAccountResponse>> Update(string номерСчета, [FromBody] ClientAccountUpdateRequest request)
    {
        var updated = await _service.UpdateAsync(номерСчета, request);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{номерСчета}")]
    public async Task<IActionResult> Delete(string номерСчета)
    {
        var result = await _service.DeleteAsync(номерСчета);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("by-client/{клиент}")]
    public async Task<ActionResult<IEnumerable<ClientAccountResponse>>> GetByClient(int клиент)
    {
        return Ok(await _service.GetByClientAsync(клиент));
    }

    [HttpGet("by-status/{статус}")]
    public async Task<ActionResult<IEnumerable<ClientAccountResponse>>> GetByStatus(string статус)
    {
        return Ok(await _service.GetByStatusAsync(статус));
    }
} 