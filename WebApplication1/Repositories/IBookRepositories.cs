using WebApplication1.Models;

namespace WebApplication1.Repositories;

public interface IBookRepositories
{
    public Task<Books> GetBooksById(int id);

    public Task AddBooksAndGenres(BooksAndGenres books);

    Task<bool> DoesBookExists(int id);
}