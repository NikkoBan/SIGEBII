using System.ComponentModel.DataAnnotations;

namespace SIGEBI.Web0.Models
{
    public class Authormodel
    {
        public int AuthorId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Nationality { get; set; }
    }


    public class GetAllAuthorResponse
    {
        public bool IsSucces { get; set; }
        public  string? message { get; set; }

        public List<Authormodel> data { get; set; } = new List<Authormodel>();
       
    }
    public class GetAuthorResponse
    {
        public bool IsSucces { get; set; }
        public string? message { get; set; }

        public Authormodel? data { get; set; }
    }

    public class AuthorEditResponse
    {
        public bool isSucces { get; set; }
        public string? message { get; set; }

        public object? data { get; set; }

    }
    public class CreateAuthorModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public required string Nationality { get; set; }
    }

    public class AuthorCreateResponse
    {
        public bool IsSucces { get; set; }
        public string? message { get; set; }

        public int data { get; set; }

    }


}

