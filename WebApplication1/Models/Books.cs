using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Books
{
    [Required]
    public int  PK { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public Genres Genres { get; set; }
    
}

public class Genres
{

    [Required]
    public string Opis { get; set; }
    
}

