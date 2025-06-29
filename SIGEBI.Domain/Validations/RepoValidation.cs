using SIGEBI.Domain.Entities;
using System;

namespace SIGEBI.Domain.Validations
{
    public static class RepoValidation
    {
        public static bool ValidarID(int id) => id > 0;

        public static bool ValidarString(string? texto, int maxLength = 255) =>
            !string.IsNullOrWhiteSpace(texto) && texto.Length <= maxLength;

        public static bool ValidarFecha(DateTime? fecha) =>
            fecha != null && fecha.Value.Year > 1900 && fecha.Value <= DateTime.Now;

        public static bool ValidarEntidad<T>(T? entidad) where T : class => entidad != null;

        public static bool ValidarAuthor(Author author)
        {
            return ValidarString(author.FirstName, 100) &&
                   ValidarString(author.LastName, 100);
        }

        public static bool ValidarBook(Book book)
        {
            return ValidarString(book.Title, 255) &&
                   ValidarString(book.ISBN, 20) &&
                   ValidarFecha(book.PublicationDate) &&
                   ValidarID(book.CategoryId) &&
                   ValidarID(book.PublisherId) &&
                   book.TotalCopies >= 0 &&
                   book.AvailableCopies >= 0 &&
                   ValidarString(book.Language, 50);
        }

        public static bool ValidarCategory(Category category)
        {
            return ValidarString(category.CategoryName, 100);
        }

        public static bool ValidarLoan(Loan loan)
        {
            return ValidarID(loan.BookId) &&
                   ValidarID(loan.UserId) &&
                   ValidarFecha(loan.LoanDate) &&
                   ValidarFecha(loan.DueDate) &&
                   (loan.ReturnDate == null || loan.ReturnDate >= loan.LoanDate) &&
                   ValidarString(loan.LoanStatus, 50);
        }

        public static bool ValidarLoanHistory(LoanHistory history)
        {
            return ValidarID(history.OriginalLoanId) &&
                   ValidarID(history.BookId) &&
                   ValidarID(history.UserId) &&
                   ValidarFecha(history.LoanDate) &&
                   ValidarFecha(history.DueDate) &&
                   ValidarFecha(history.ReturnDate) &&
                   ValidarString(history.FinalStatus, 50);
        }

        public static bool ValidarBookAuthor(BookAuthor bookAuthor)
        {
            return ValidarID(bookAuthor.AuthorId) &&
                   ValidarID(bookAuthor.ID);
        }
    }
}
