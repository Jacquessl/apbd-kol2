using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    private IDbService _dbServiceImplementation;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<bool> DoesCharacterExist(int id)
    {
        return await _context.Characters.AnyAsync(e => e.Id == id);
    }
    public async Task<bool> DoesItemExist(int id)
    {
        return await _context.Items.AnyAsync(e => e.Id == id);
    }

    public async Task<Item> GetItemData(int id)
    {
        return _context.Items.Where(e => e.Id == id).First();
    }

    public async Task<bool> CanUdzwignac(int id, int weight)
    {
        var characterRes = _context.Characters.Where(e => e.Id == id).First();

        return characterRes.MaxWeight > (weight + characterRes.CurrentWeight);
    }

    public async Task AddItems(int id, ICollection<NewItemDTO> newItemDto)
    {
        int totalWeight = 0;
        foreach(var newItem in newItemDto)
        {
            totalWeight += _context.Items.Where(e => e.Id == newItem.Id).First().Weight * newItem.Amount;
            await _context.Backpacks.AddAsync(new Backpack()
            {
                Amount = newItem.Amount,
                ItemId = newItem.Id,
                CharacterId = id
            });
        }

        _context.Characters.Where(e => e.Id == id).First().CurrentWeight += totalWeight;
    }

    public async Task<CharacterDTO> GetCharacterData(int id)
    {
        var resCharacter = _context.Characters.Where(e => e.Id == id).First();
        var result = new CharacterDTO()
        {
            FirstName = resCharacter.FirstName,
            LastName = resCharacter.LastName,
            MaxWeight = resCharacter.MaxWeight,
            CurrentWeight = 0
        };
        foreach (var resCharacterBackpack in _context.Backpacks.Where(e=>e.CharacterId==id))
        {
            result.CurrentWeight = resCharacterBackpack.Item.Weight * resCharacterBackpack.Amount;
            result.Backpacks.Add(resCharacterBackpack);
        }
        foreach (var resCharacterCharacterTitle in _context.CharacterTitles.Where(e=>e.CharacterId==id))
        {
            result.CharacterTitles.Add(resCharacterCharacterTitle);
        }
        return result;
    }
}