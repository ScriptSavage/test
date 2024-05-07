using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class BooksRepositories : IBookRepositories
{

    private readonly IConfiguration _configuration;

    public BooksRepositories(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Books> GetBooksById(int id)
    {

        var query = @"SELECT 
[books].PK AS BooksID ,
[books].title AS BookTitle ,
[genres].name AS Opis 
FROM Books
INNER JOIN books_genres ON books.PK = [books_genres].FK_book
INNER JOIN genres ON  [books_genres].FK_genre = genres.PK
WHERE [genres].PK = @ID";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        await connection.OpenAsync();
        command.Parameters.AddWithValue("@ID", id);

        var reader = await command.ExecuteReaderAsync();

        var IdBookOrdinal = reader.GetOrdinal("BooksID");
        var BookTitleOrdinal = reader.GetOrdinal("BookTitle");
        var BookDesOrdinal = reader.GetOrdinal("Opis");

        Books books = null;

        while (await reader.ReadAsync())
        {
            books = new Books()
            {
                PK = reader.GetInt32(IdBookOrdinal),
                Title = reader.GetString(BookTitleOrdinal),
                Genres = new Genres()
                {
                    Opis = reader.GetString(BookDesOrdinal)
                }
            };
        }
        return books;
    }

    public async Task AddBooksAndGenres(BooksAndGenres books)
    {
        var query = @"INSERT INTO books VALUES (@Name);";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        

        command.Parameters.AddWithValue("@Name", books.Title);
        
        await connection.OpenAsync();

        var transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;


        try
        {
           var id = await command.ExecuteScalarAsync();

           foreach (var proc  in books.ProcGenresEnumerable)
           {
               command.Parameters.Clear();
               command.CommandText = @"INSERT INTO genres VALUES @Opis; ";
               command.Parameters.AddWithValue("@Opis", proc.opis2);
               
               await command.ExecuteNonQueryAsync();
           }
           await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
        
    }

    public async Task<bool> DoesBookExists(int id)
    {
        var query = @"SELECT 1 FROM BOOKS WHERE PK = @ID";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        
        await connection.OpenAsync();

        var res = await command.ExecuteScalarAsync();

        return res is not null;
    }
}