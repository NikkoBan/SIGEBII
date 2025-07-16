
using Microsoft.Extensions.Logging;

using SIGEBI.Persistence.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Domain.IRepository;
using SIGEBI.Persistence.Context;


using System.Data;
using Microsoft.EntityFrameworkCore;




namespace SIGEBI.Persistence.Repositories
{
    public class AuthorRepository : BaseRepositoryEf<Authors>, IAuthorRepository
    {
        public AuthorRepository(SIGEBIContext context, ILogger<AuthorRepository> logger)
            : base(context, logger)
        {
        }

        // Verificar duplicados 
        public async Task<bool> CheckDuplicateAuthorAsync(string firstName, string lastName, DateTime? birthDate)
        {
            try
            {
                return await _dbSet.AnyAsync(a =>
                    a.FirstName == firstName &&
                    a.LastName == lastName &&
                    a.BirthDate == birthDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar duplicidad de autor.");
                return false;
            }
        }
        //Dublicados al actualizar
        public async Task<bool> CheckDuplicateAuthorForUpdateAsync(int id, string firstName, string lastName, DateTime? birthDate)
        {
            try
            {
                return await _dbSet.AnyAsync(a =>
                    a.Id != id &&
                    a.FirstName == firstName &&
                    a.LastName == lastName &&
                    a.BirthDate == birthDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al verificar duplicidad de autor para actualización de ID {id}.");
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetGenresByAuthorAsync(int authorId)
        {
            try
            {
                var genres = await _context.BookAuthor
                    .Include(ba => ba.Book)
                        .ThenInclude(b => b!.Category)
                    .Where(ba => ba.AuthorId == authorId && ba.Book != null && ba.Book.Category != null)
                    .Select(ba => ba.Book!.Category!.CategoryName)
                    .Where(categoryName => categoryName != null)
                    .Distinct()
                    .ToListAsync();

                return genres!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener géneros para el autor con ID {authorId}.");
                return Enumerable.Empty<string>();
            }
        }
    }
}
