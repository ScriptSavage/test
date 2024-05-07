using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;


[ApiController]
[Route("/api/[controller]")]
public class BookController : ControllerBase
{

    public IBookRepositories _BookRepositories;

    public BookController(IBookRepositories bookRepositories)
    {
        _BookRepositories = bookRepositories;
    }

    [HttpGet]
    [Route("id:int")]
    public async Task<IActionResult> GetBookByID(int id)
    {

        if (!await _BookRepositories.DoesBookExists(id))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        
        var x = await _BookRepositories.GetBooksById(id);
        return Ok(x);
        
        //fwuifwgyuwgwb
    }


    [HttpPost]
    public async Task<IActionResult> AddBook(BooksAndGenres books)
    {
        var x =  _BookRepositories.AddBooksAndGenres(books);
        return Ok(x);
    }

}