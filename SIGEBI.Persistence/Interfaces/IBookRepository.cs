using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book, int>
    {
        Task<Book?> GetBookByISBN(string isbn);

        Task<List<Book>> GetBooksByCategory(int categoryId);

        Task<List<Book>> GetAvailableBooks();

        // Nuevos métodos propuestos:

        /// <summary>
        /// Busca libros cuyo título contenga la cadena dada (sin importar mayúsculas).
        /// </summary>
        Task<List<Book>> SearchBooksByTitle(string title);

        /// <summary>
        /// Obtiene libros que han sido marcados como borrados (soft delete).
        /// </summary>
        Task<List<Book>> GetDeletedBooks();

        /// <summary>
        /// Restaura un libro que estaba marcado como borrado.
        /// </summary>
        Task<OperationResult> RestoreDeletedBook(int bookId);
    }
}

