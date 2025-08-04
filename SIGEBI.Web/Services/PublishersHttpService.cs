using SIGEBI.Web.Models.Publishers;
using Microsoft.Extensions.Logging;

namespace SIGEBI.Web.Services
{
    public class PublishersHttpService : IPublishersHttpService
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<PublishersHttpService> _logger;
        private readonly string _apiBaseUrl;

        public PublishersHttpService(
            IHttpService httpService,
            ILogger<PublishersHttpService> logger,
            IConfiguration configuration)
        {
            _httpService = httpService;
            _logger = logger;
            _apiBaseUrl = configuration["ApiSettings:PublishersBaseUrl"]
                ?? throw new ArgumentNullException("ApiSettings:PublishersBaseUrl");
        }

        public async Task<List<PublishersViewModel>> GetAllAsync()
        {
            try
            {
                return await _httpService.GetAsync<List<PublishersViewModel>>(_apiBaseUrl) ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de editoriales.");
                return new List<PublishersViewModel>();
            }
        }

        public async Task<PublishersViewModel?> GetByIdAsync(int id)
        {
            try
            {
                return await _httpService.GetAsync<PublishersViewModel>($"{_apiBaseUrl}/{id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la editorial con ID {Id}.", id);
                return null;
            }
        }

        public async Task<ApiResponseModel<PublishersViewModel>?> CreateAsync(PublisherCreateModel model)
        {
            try
            {
                return await _httpService.PostAsync<PublisherCreateModel, ApiResponseModel<PublishersViewModel>>(_apiBaseUrl, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la editorial. Datos: {@Model}", model);
                return new ApiResponseModel<PublishersViewModel>
                {
                    success = false,
                    failed = true,
                    message = "Error al crear la editorial.",
                    data = null!
                };
            }
        }

        public async Task<ApiResponseModel<PublishersViewModel>?> UpdateAsync(PublisherUpdateModel model)
        {
            try
            {
                return await _httpService.PutAsync<PublisherUpdateModel, ApiResponseModel<PublishersViewModel>>($"{_apiBaseUrl}/{model.id}", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la editorial con ID {Id}. Datos: {@Model}", model.id, model);
                return new ApiResponseModel<PublishersViewModel>
                {
                    success = false,
                    failed = true,
                    message = "Error al actualizar la editorial.",
                    data = null!
                };
            }
        }
    }
}