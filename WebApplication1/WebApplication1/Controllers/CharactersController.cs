using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
public class CharactersController : ControllerBase
{
    private readonly IDbService _dbService;

    public CharactersController(IDbService dbService)
    {
        _dbService = dbService;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCharacterInfo(int id)
    {
        if (!await _dbService.DoesCharacterExist(id))
        {
            return NotFound("Character doesn't exist");
        }

        var result = await _dbService.GetCharacterData(id);
        return Ok(result);
    }

    [HttpPost("{id}/backpacks")]
    public async Task<IActionResult> AddItem(int id, ICollection<NewItemDTO> newItemDtos)
    {
        if (!await _dbService.DoesCharacterExist(id))
        {
            return NotFound("Character doesn't exist");
        }

        int totalWeight = 0;
        foreach (var newItemDto in newItemDtos)
        {
            if (!await _dbService.DoesItemExist(newItemDto.Id))
            {
                return NotFound("Item doesn't exist");
            }

            totalWeight += _dbService.GetItemData(id).Result.Weight;
        }

        if (!await _dbService.CanUdzwignac(id, totalWeight))
        {
            return BadRequest("Character can't udziwgnac tyle ciezaru");
        }

        await _dbService.AddItems(id, newItemDtos);
        return Created();
    }
}