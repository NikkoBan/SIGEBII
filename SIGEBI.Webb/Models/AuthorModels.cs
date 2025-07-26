namespace SIGEBI.Web.Models
{
    public class Authormodel
    {
        public int authorId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime birthDate { get; set; }
        public string nationality { get; set; }
    }

    public class GetAllAuthorResponse
    {
        public bool isSucces { get; set; }
        public string message { get; set; }

        public List<Authormodel> data { get; set; }
        public class GetAuthorResponse
        {
            public bool isSucces { get; set; }
            public string message { get; set; }

            public Authormodel data { get; set; }
        }
    }
}

