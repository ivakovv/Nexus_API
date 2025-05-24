namespace Nexus_API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Nexus_API.Models.DTOs;
using Nexus_API.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class CardsController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardsController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CardResponse>>> GetCards()
    {
        var cards = await _cardService.GetAllCardsAsync();
        return Ok(cards);
    }

    [HttpGet("{номерКарты}")]
    public async Task<ActionResult<CardResponse>> GetCard(string номерКарты)
    {
        var card = await _cardService.GetCardByIdAsync(номерКарты);
        return card == null ? NotFound() : Ok(card);
    }

    [HttpPost]
    public async Task<ActionResult<CardResponse>> CreateCard([FromBody] CardCreateRequest request)
    {
        try
        {
            var createdCard = await _cardService.CreateCardAsync(request);
            return CreatedAtAction(nameof(GetCard), new { номерКарты = createdCard.НомерКарты }, createdCard);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{номерКарты}")]
    public async Task<IActionResult> UpdateCard(string номерКарты, [FromBody] CardUpdateRequest request)
    {
        try
        {
            var updatedCard = await _cardService.UpdateCardAsync(номерКарты, request);
            return updatedCard == null ? NotFound() : Ok(updatedCard);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{номерКарты}")]
    public async Task<IActionResult> DeleteCard(string номерКарты)
    {
        try
        {
            var result = await _cardService.DeleteCardAsync(номерКарты);
            return result ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("by-account/{номерСчета}")]
    public async Task<ActionResult<IEnumerable<CardResponse>>> GetCardsByAccount(string номерСчета)
    {
        var cards = await _cardService.GetCardsByAccountAsync(номерСчета);
        return Ok(cards);
    }

    [HttpGet("by-status/{статус}")]
    public async Task<ActionResult<IEnumerable<CardResponse>>> GetCardsByStatus(string статус)
    {
        var cards = await _cardService.GetCardsByStatusAsync(статус);
        return Ok(cards);
    }
}