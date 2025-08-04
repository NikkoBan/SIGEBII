using SIGEBI.Web0.Interfaz.Author;
using SIGEBI.Web0.Models.Author;



namespace SIGEBI.Web0.Services.Author
{
   
    public class AuthorWebService : BaseWebService<Authormodel, CreateAuthorModel, EditAuthorModel, IAuthorWeb>, IAuthorWebService
    {

        public AuthorWebService(IAuthorWeb repository, ILogger<AuthorWebService> logger)
            : base(repository, logger)
        {
        }

      
    }
}
