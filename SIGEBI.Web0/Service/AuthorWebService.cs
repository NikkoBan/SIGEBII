using SIGEBI.Web0.Interfaz.Author;
using SIGEBI.Web0.Models.Author;

namespace SIGEBI.Web0.Services.Author
{
    public class AuthorWebService : IAuthorWebService
    {
        private readonly IAuthorWeb _authorWebRepository;
        private readonly ILogger<AuthorWebService> _logger;

        public AuthorWebService(IAuthorWeb authorWebRepository, ILogger<AuthorWebService> logger)
        {
            _authorWebRepository = authorWebRepository;
            _logger = logger;
        }

        public async Task<List<Authormodel>> GetAllAuthorsAsync()
        {
            try
            {
                var authors = await _authorWebRepository.GetAllAsync();
                return authors ?? new List<Authormodel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los autores.");
                throw;
            }
        }

        public async Task<Authormodel?> GetAuthorByIdAsync(int id)
        {
            try
            {
                return await _authorWebRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el autor con ID: {AuthorId}", id);
                throw;
            }
        }

        public async Task<bool> CreateAuthorAsync(CreateAuthorModel model)
        {
            try
            {
                return await _authorWebRepository.CreateAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo autor.");
                throw;
            }
        }

        public async Task<bool> UpdateAuthorAsync(int id, EditAuthorModel model)
        {
            try
            {
                return await _authorWebRepository.UpdateAsync(id, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el autor con ID: {AuthorId}", id);
                throw;
            }
        }

        public async Task<EditAuthorModel?> GetEditAuthorModelByIdAsync(int id)
        {
            try
            {
                return await _authorWebRepository.GetEditModelByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el modelo de edición para el autor con ID: {AuthorId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            try
            {
                return await _authorWebRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el autor con ID: {AuthorId}", id);
                throw;
            }
        }
    }
}
