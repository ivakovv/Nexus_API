using Microsoft.AspNetCore.Mvc;
using Nexus_API.Interfaces;
using Nexus_API.Models.DTOs;

namespace Nexus_API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransactionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTransactions()
    {
        var transactions = await _transactionService.GetAllTransactionsAsync();
        return Ok(transactions);
    }

    [HttpGet("{кодТранзакции}")]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransactionByCode(string кодТранзакции)
    {
        try
        {
            var transaction = await _transactionService.GetTransactionByCodeAsync(кодТранзакции);
            return Ok(transaction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<TransactionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchTransactions([FromQuery] string term)
    {
        var transactions = await _transactionService.SearchTransactionsAsync(term);
        return Ok(transactions);
    }

    [HttpGet("status/{статус}")]
    [ProducesResponseType(typeof(IEnumerable<TransactionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactionsByStatus(string статус)
    {
        var transactions = await _transactionService.GetTransactionsByStatusAsync(статус);
        return Ok(transactions);
    }

    [HttpGet("type/{типТранзакции}")]
    [ProducesResponseType(typeof(IEnumerable<TransactionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactionsByType(string типТранзакции)
    {
        var transactions = await _transactionService.GetTransactionsByTypeAsync(типТранзакции);
        return Ok(transactions);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateRequest request)
    {
        try
        {
            var createdTransaction = await _transactionService.CreateTransactionAsync(request);
            return CreatedAtAction(nameof(GetTransactionByCode), new { кодТранзакции = createdTransaction.КодТранзакции }, createdTransaction);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{кодТранзакции}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTransaction(string кодТранзакции, [FromBody] TransactionUpdateRequest request)
    {
        try
        {
            await _transactionService.UpdateTransactionAsync(кодТранзакции, request);
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

    [HttpDelete("{кодТранзакции}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTransaction(string кодТранзакции)
    {
        try
        {
            await _transactionService.DeleteTransactionAsync(кодТранзакции);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
} 