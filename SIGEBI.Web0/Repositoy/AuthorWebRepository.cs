using SIGEBI.Web0.Interfaz;
using SIGEBI.Web0.Models.Author;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using SIGEBI.Web0.Interfaz.Author;

namespace SIGEBI.Web0.Repositories.AuthorRepository
{
    public class AuthorWebRepository : BaseWebRepository<Authormodel, CreateAuthorModel, EditAuthorModel>, IAuthorWeb
    {
        public AuthorWebRepository(HttpClient httpClient, ILogger<AuthorWebRepository> logger)
            : base(httpClient, logger, "api/Author")
        {
        }

        public override async Task<EditAuthorModel?> GetEditModelByIdAsync(int id)
        {
            var authorModel = await GetByIdAsync(id);
            if (authorModel == null)
            {
                return null;
            }

            return new EditAuthorModel
            {
                AuthorId = authorModel.AuthorId,
                FirstName = authorModel.FirstName,
                LastName = authorModel.LastName,
                BirthDate = authorModel.BirthDate,
                Nationality = authorModel.Nationality
            };
        }
    }
}
