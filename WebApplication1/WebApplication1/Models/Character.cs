using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

[Table("characters")]
public class Character
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)] 
    public string FirstName { get; set; } = string.Empty!;
    [MaxLength(100)] 
    public string LastName { get; set; } = string.Empty!;
    public int CurrentWeight { get; set; }
    public int MaxWeight { get; set; }
    public ICollection<Backpack> Backpacks { get; set; } = new HashSet<Backpack>();
    public ICollection<CharacterTitle> CharacterTitles { get; set; } = new HashSet<CharacterTitle>();
    
}