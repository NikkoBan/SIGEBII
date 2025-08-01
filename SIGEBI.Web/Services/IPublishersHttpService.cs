using SIGEBI.Web.Models.Publishers;

namespace SIGEBI.Web.Services
{
    public interface IPublishersHttpService
    {
        Task<List<PublishersViewModel>> GetAllAsync();
        Task<PublishersViewModel?> GetByIdAsync(int id);
        Task<ApiResponseModel<PublishersViewModel>?> CreateAsync(PublisherCreateModel model);
        Task<ApiResponseModel<PublishersViewModel>?> UpdateAsync(PublisherUpdateModel model);

    }
}
