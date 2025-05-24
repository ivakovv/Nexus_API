using Microsoft.AspNetCore.Mvc;
using Nexus_API.Interfaces;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CreditsController : ControllerBase
{
    private readonly ICreditService _creditService;

    public CreditsController(ICreditService creditService)
    {
        _creditService = creditService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CreditResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCredits()
    {
        var credits = await _creditService.GetAllCreditsAsync();
        return Ok(credits);
    }

    [HttpGet("{номерДоговора}")]
    [ProducesResponseType(typeof(CreditResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCreditByNumber(string номерДоговора)
    {
        try
        {
            var credit = await _creditService.GetCreditByNumberAsync(номерДоговора);
            return Ok(credit);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<CreditResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchCredits([FromQuery] string term)
    {
        var credits = await _creditService.SearchCreditsAsync(term);
        return Ok(credits);
    }

    [HttpGet("status/{статус}")]
    [ProducesResponseType(typeof(IEnumerable<CreditResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCreditsByStatus(string статус)
    {
        var credits = await _creditService.GetCreditsByStatusAsync(статус);
        return Ok(credits);
    }

    [HttpGet("type/{видКредита}")]
    [ProducesResponseType(typeof(IEnumerable<CreditResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCreditsByType(int видКредита)
    {
        var credits = await _creditService.GetCreditsByTypeAsync(видКредита);
        return Ok(credits);
    }

    [HttpGet("{номерДоговора}/schedule")]
    [ProducesResponseType(typeof(IEnumerable<PaymentScheduleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentSchedule(string номерДоговора)
    {
        try
        {
            var schedule = await _creditService.GetPaymentScheduleAsync(номерДоговора);
            return Ok(schedule);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreditResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCredit([FromBody] CreditCreateRequest request)
    {
        try
        {
            var createdCredit = await _creditService.CreateCreditAsync(request);
            return CreatedAtAction(nameof(GetCreditByNumber), new { номерДоговора = createdCredit.НомерДоговора }, createdCredit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{номерДоговора}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCredit(string номерДоговора, [FromBody] CreditUpdateRequest request)
    {
        try
        {
            await _creditService.UpdateCreditAsync(номерДоговора, request);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{номерДоговора}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCredit(string номерДоговора)
    {
        try
        {
            await _creditService.DeleteCreditAsync(номерДоговора);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("by-account/{номерСчета}")]
    [ProducesResponseType(typeof(IEnumerable<CreditResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCreditsByAccount(string номерСчета)
    {
        var credits = await _creditService.GetCreditsByAccountAsync(номерСчета);
        return Ok(credits);
    }
} 