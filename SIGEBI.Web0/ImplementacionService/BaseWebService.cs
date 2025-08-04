using SIGEBI.Web0.Interfaz;


namespace SIGEBI.Web0.Services
{

    public abstract class BaseWebService<TModel, TCreateModel, TEditModel, TRepository> : IBaseWebService<TModel, TCreateModel, TEditModel>
         where TModel : class
         where TCreateModel : class
         where TEditModel : class
         where TRepository : IBaseWebRepository<TModel, TCreateModel, TEditModel>
    {
        protected readonly TRepository _repository;
        protected readonly ILogger _logger;

        public BaseWebService(TRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
        {
            try
            {
                var models = await _repository.GetAllAsync();
                return models ?? new List<TModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener todas las entidades.");
                throw;
            }
        }

        public virtual async Task<TModel?> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener la entidad con ID: {Id}", id);
                throw;
            }
        }

        public virtual async Task<TEditModel?> GetEditModelByIdAsync(int id)
        {
            try
            {
                return await _repository.GetEditModelByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al obtener el modelo de edición para la entidad con ID: {Id}", id);
                throw;
            }
        }

        public virtual async Task<bool> CreateAsync(TCreateModel model)
        {
            try
            {
                return await _repository.CreateAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al crear una nueva entidad.");
                throw;
            }
        }

        public virtual async Task<bool> UpdateAsync(int id, TEditModel model)
        {
            try
            {
                return await _repository.UpdateAsync(id, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al actualizar la entidad con ID: {Id}", id);
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio al eliminar la entidad con ID: {Id}", id);
                throw;
            }
        }
    }
}


