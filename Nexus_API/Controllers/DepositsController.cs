using Microsoft.AspNetCore.Mvc;
using Nexus_API.Interfaces;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DepositsController : ControllerBase
{
    private readonly IDepositService _depositService;

    public DepositsController(IDepositService depositService)
    {
        _depositService = depositService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DepositResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDeposits()
    {
        var deposits = await _depositService.GetAllDepositsAsync();
        return Ok(deposits);
    }

    [HttpGet("{номерДепозитногоДоговора}")]
    [ProducesResponseType(typeof(DepositResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDepositByNumber(string номерДепозитногоДоговора)
    {
        try
        {
            var deposit = await _depositService.GetDepositByNumberAsync(номерДепозитногоДоговора);
            return Ok(deposit);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<DepositResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchDeposits([FromQuery] string term)
    {
        var deposits = await _depositService.SearchDepositsAsync(term);
        return Ok(deposits);
    }

    [HttpGet("status/{статус}")]
    [ProducesResponseType(typeof(IEnumerable<DepositResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDepositsByStatus(string статус)
    {
        var deposits = await _depositService.GetDepositsByStatusAsync(статус);
        return Ok(deposits);
    }

    [HttpGet("type/{типДепозита}")]
    [ProducesResponseType(typeof(IEnumerable<DepositResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDepositsByType(string типДепозита)
    {
        var deposits = await _depositService.GetDepositsByTypeAsync(типДепозита);
        return Ok(deposits);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DepositResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDeposit([FromBody] DepositCreateRequest request)
    {
        try
        {
            var createdDeposit = await _depositService.CreateDepositAsync(request);
            return CreatedAtAction(nameof(GetDepositByNumber), new { номерДепозитногоДоговора = createdDeposit.НомерДепозитногоДоговора }, createdDeposit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{номерДепозитногоДоговора}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDeposit(string номерДепозитногоДоговора, [FromBody] DepositUpdateRequest request)
    {
        try
        {
            await _depositService.UpdateDepositAsync(номерДепозитногоДоговора, request);
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

    [HttpDelete("{номерДепозитногоДоговора}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDeposit(string номерДепозитногоДоговора)
    {
        try
        {
            await _depositService.DeleteDepositAsync(номерДепозитногоДоговора);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("by-account/{номерСчета}")]
    [ProducesResponseType(typeof(IEnumerable<DepositResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDepositsByAccount(string номерСчета)
    {
        var deposits = await _depositService.GetDepositsByAccountAsync(номерСчета);
        return Ok(deposits);
    }
} 