using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class BooksAndGenres
{
    [Required]
    public int  PK { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public Genres Genres { get; set; }

    public IEnumerable<ProcGenres> ProcGenresEnumerable { get; set; } = new List<ProcGenres>();

}

public class ProcGenres
{
    public int  ID { get; set; }
    public string opis2 { get; set; }
}
