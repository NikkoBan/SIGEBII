using Microsoft.EntityFrameworkCore;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Validations;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Context;
using SIGEBI.Persistence.Interfaces;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class BookRepository : BaseRepository<Book, int>, IBookRepository
    {
        private readonly SIGEBIDbContext _context;

        public BookRepository(SIGEBIDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Book?> GetBookByISBN(string isbn)
        {
            return await _context.Books
                .Where(b => !b.Borrado && b.ISBN == isbn)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetAvailableBooks()
        {
            return await _context.Books
                .Where(b => !b.Borrado && b.Stock > 0)
                .ToListAsync();
        }

        public async Task<List<Book>> SearchBooksByTitle(string title)
        {
            return await _context.Books
                .Where(b => !b.Borrado && b.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Book>> GetDeletedBooks()
        {
            return await _context.Books
                .Where(b => b.Borrado)
                .ToListAsync();
        }

        public async Task<OperationResult<Book>> RestoreDeletedBook(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || !book.Borrado)
            {
                return OperationResult<Book>.FailResult("Libro no encontrado o no está eliminado.");
            }

            book.Borrado = false;

            try
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return OperationResult<Book>.SuccessResult(book, "Libro restaurado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.FailResult($"Error restaurando libro: {ex.Message}");
            }
        }

        public override async Task<bool> Exists(Expression<Func<Book, bool>> filter)
        {
            return await _context.Books.AnyAsync(filter);
        }

        public override async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                .Where(b => !b.Borrado)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<Book?> GetEntityByIdAsync(int id)
        {
            if (!RepoValidation.ValidarID(id))
                return null;

            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.ID == id && !b.Borrado);
        }

        public async Task<List<Book>> GetBooksByCategory(int categoryId)
        {
            return await _context.Books
                .Where(b => b.CategoryId == categoryId && !b.Borrado)
                .Include(b => b.Publisher)
                .AsNoTracking()
                .ToListAsync();
        }

        public override async Task<OperationResult<Book>> SaveEntityAsync(Book entity)
        {
            try
            {
                _context.Books.Add(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Book>.SuccessResult(entity, "Libro guardado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.FailResult($"Error guardando libro: {ex.Message}");
            }
        }

        public override async Task<OperationResult<Book>> UpdateEntityAsync(Book entity)
        {
            try
            {
                _context.Books.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Book>.SuccessResult(entity, "Libro actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.FailResult($"Error actualizando libro: {ex.Message}");
            }
        }

        public override async Task<OperationResult<Book>> RemoveEntityAsync(Book entity)
        {
            try
            {
                _context.Books.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<Book>.SuccessResult(entity, "Libro eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.FailResult($"Error eliminando libro: {ex.Message}");
            }
        }
    }
}
