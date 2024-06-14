using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services;

public interface IDbService
{
    Task<bool> DoesCharacterExist(int id);
    Task<CharacterDTO> GetCharacterData(int id);
    Task<bool> DoesItemExist(int id);
    Task<Item> GetItemData(int id);
    Task<bool> CanUdzwignac(int id, int weight);
    Task AddItems(int id, ICollection<NewItemDTO> newItemDto);
}