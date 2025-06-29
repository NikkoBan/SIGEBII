using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Persistence.Repositories.SIGEBI.Persistence.Repositories;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace SIGEBI.Persistence.Repositories
{
    public class BookAuthorRepository : BaseRepositoryAdo<BooksAuthors>, IBookAuthorRepository
    {
        public BookAuthorRepository(string connectionString, ILogger<BaseRepositoryAdo<BooksAuthors>> logger)
            : base(connectionString, logger) { }

        public override Task<OperationResult> CreateAsync(BooksAuthors entity)
        {
            throw new NotImplementedException();
        }

        public override Task<OperationResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

       

        public override Task<OperationResult> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public override Task<OperationResult> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<OperationResult> UpdateAsync(BooksAuthors entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckDuplicateBookAuthorCombinationAsync(int bookId, int authorId)
        {
            try
            {
               
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("SELECT COUNT(1) FROM BooksAuthors WHERE BookId = @BookId AND AuthorId = @AuthorId", conn))
                    {
                        cmd.Parameters.AddWithValue("@BookId", bookId);
                        cmd.Parameters.AddWithValue("@AuthorId", authorId);
                        return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking duplicate for BookId {bookId}, AuthorId {authorId}.");
                return false;
            }
        }

        public override Task<OperationResult> CreateAsync(AuditableEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
