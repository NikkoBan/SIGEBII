using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class BookAuthorRepository : BaseRepository<BookAuthor, int>, IBookAuthorRepository
    {
        private readonly SIGEBIDbContext _context;

        public BookAuthorRepository(SIGEBIDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BookAuthor>> GetByAuthorId(int authorId)
        {
            return await _context.BookAuthors
                .Where(ba => ba.AuthorId == authorId)
                .Include(ba => ba.Author)
                .Include(ba => ba.Book)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<BookAuthor>> GetByBookId(int bookId)
        {
            return await _context.BookAuthors
                .Where(ba => ba.ID == bookId)
                .Include(ba => ba.Author)
                .Include(ba => ba.Book)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<BookAuthor?> GetEntityByIdAsync(int id)
        {
            return await _context.BookAuthors
                .Include(ba => ba.Author)
                .Include(ba => ba.Book)
                .FirstOrDefaultAsync(ba => ba.ID == id);
        }

        public override async Task<bool> Exists(Expression<Func<BookAuthor, bool>> filter)
        {
            return await _context.BookAuthors.AnyAsync(filter);
        }

        public override async Task<List<BookAuthor>> GetAllAsync()
        {
            return await _context.BookAuthors
                .Include(ba => ba.Author)
                .Include(ba => ba.Book)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<OperationResult<BookAuthor>> SaveEntityAsync(BookAuthor entity)
        {
            try
            {
                _context.BookAuthors.Add(entity);
                await _context.SaveChangesAsync();
                return OperationResult<BookAuthor>.SuccessResult(entity, "BookAuthor guardado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<BookAuthor>.FailResult($"Error al guardar BookAuthor: {ex.Message}");
            }
        }

        public override async Task<OperationResult<BookAuthor>> UpdateEntityAsync(BookAuthor entity)
        {
            try
            {
                _context.BookAuthors.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<BookAuthor>.SuccessResult(entity, "BookAuthor actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<BookAuthor>.FailResult($"Error al actualizar BookAuthor: {ex.Message}");
            }
        }

        public override async Task<OperationResult<BookAuthor>> RemoveEntityAsync(BookAuthor entity)
        {
            try
            {
                _context.BookAuthors.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<BookAuthor>.SuccessResult(entity, "BookAuthor eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<BookAuthor>.FailResult($"Error al eliminar BookAuthor: {ex.Message}");
            }
        }
    }
}
