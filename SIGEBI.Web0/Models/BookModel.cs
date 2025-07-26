namespace SIGEBI.Web0.Models
{

    public class BookModel
    {
        public int bookId { get; set; }
        public required string title { get; set; }
        public required string isbn { get; set; }
        public DateTime publicationDate { get; set; }
        public int categoryId { get; set; }
        public required string categoryName { get; set; }
        public int publisherId { get; set; }
        public required string publisherName { get; set; }
        public required string language { get; set; }
        public required string summary { get; set; }
        public int totalCopies { get; set; }
        public int availableCopies { get; set; }
        public required string generalStatus { get; set; }
        public bool isAvailable { get; set; }
    }

    public class GetAllBookResponse
    {
        public bool IsSuccess { get; set; }
        public string ?message { get; set; }
        public List<BookModel> ?data { get; set; }
    }

    public class GetBookResponse
    {
        public bool IsSuccess { get; set; }
        public string? message { get; set; }
        public BookModel ?data { get; set; }
    }

    public class BookCreateResponse
    {
        public bool IsSuccess { get; set; }
        public string? message { get; set; }
        public int data { get; set; } 
    }

    public class BookEditResponse
    {
        public bool IsSuccess { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }
    }

    public class CreateBookModel
    {

        public required string title { get; set; }
        public required string isbn { get; set; }
        public DateTime? publicationDate { get; set; } 
        public int categoryId { get; set; }  
        public int publisherId { get; set; }  
        public required string language { get; set; }
        public required string summary { get; set; }
        public int totalCopies { get; set; }
        public int availableCopies { get; set; }
        public required string generalStatus { get; set; }
        public bool isAvailable { get; set; }
    }
}


