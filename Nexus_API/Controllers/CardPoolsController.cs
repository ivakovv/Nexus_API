using Microsoft.AspNetCore.Mvc;
using Nexus_API.Interfaces;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CardPoolsController : ControllerBase
{
    private readonly ICardPoolService _service;

    public CardPoolsController(ICardPoolService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CardPoolResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{номерКарты}")]
    public async Task<ActionResult<CardPoolResponse>> GetByCardNumber(string номерКарты)
    {
        var pool = await _service.GetByCardNumberAsync(номерКарты);
        return pool == null ? NotFound() : Ok(pool);
    }

    [HttpPost]
    public async Task<ActionResult<CardPoolResponse>> Create([FromBody] CardPoolCreateRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetByCardNumber), new { номерКарты = created.НомерКарты }, created);
    }

    [HttpPut("{номерКарты}")]
    public async Task<ActionResult<CardPoolResponse>> Update(string номерКарты, [FromBody] CardPoolUpdateRequest request)
    {
        var updated = await _service.UpdateAsync(номерКарты, request);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{номерКарты}")]
    public async Task<IActionResult> Delete(string номерКарты)
    {
        var result = await _service.DeleteAsync(номерКарты);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("by-account/{номерСчета}")]
    public async Task<ActionResult<IEnumerable<CardPoolResponse>>> GetByAccount(string номерСчета)
    {
        return Ok(await _service.GetByAccountAsync(номерСчета));
    }

    [HttpGet("by-status/{статус}")]
    public async Task<ActionResult<IEnumerable<CardPoolResponse>>> GetByStatus(string статус)
    {
        return Ok(await _service.GetByStatusAsync(статус));
    }
} 