

//using AutoMapper;
//using Microsoft.Extensions.Logging;
//using SIGEBI.Application.Contracts.Service;
//using SIGEBI.Application.Dtos.CategoryDto;
//using SIGEBI.Application.Services;
//using SIGEBI.Domain.Base;
//using SIGEBI.Domain.Entities.Configuration;
//using SIGEBI.Persistence.IRepository.SIGEBI.Persistence.IRepository;

//namespace SIGEBI.Application.service
//{
//    public class CategoryService : BaseService<CreateCategoryDTO, UpdateCategoryDTO, Categories, ICategoryRepository>, ICategoryService
//    {
//        private readonly ICategoryRepository _repository;
//        private readonly IMapper _mapper;
//        private readonly ILogger<CategoryService> _logger;

//        public CategoryService(ICategoryRepository repository, IMapper mapper, ILogger<CategoryService> logger)
//            : base(repository, mapper)
//        {
//            _repository = repository;
//            _mapper = mapper;
//            _logger = logger;
//        }

//        public async Task<bool> CheckDuplicateCategoryNameAsync(string name, int? excludeId = null)
//        {
//            return await _repository.CheckDuplicateCategoryNameAsync(name, excludeId);
//        }

//        public override async Task<OperationResult> CreateAsync(CreateCategoryDTO dto)
//        {
//            if (string.IsNullOrWhiteSpace(dto.CategoryName))
//                return new OperationResult { Success = false, Message = "El nombre de la categoría es obligatorio." };

//            var exists = await _repository.CheckDuplicateCategoryNameAsync(dto.CategoryName);
//            if (exists)
//                return new OperationResult { Success = false, Message = "Ya existe una categoría con ese nombre." };

//            var category = _mapper.Map<Categories>(dto);
//            category.IsDeleted = false;
//            return await _repository.CreateAsync(category);
           

//        }

//        public override async Task<OperationResult> UpdateAsync(int id, UpdateCategoryDTO dto)
//        {
//            if (id != dto.CategoryId)
//                return new OperationResult { Success = false, Message = "El ID de la categoría no coincide." };

//            var exists = await _repository.CheckDuplicateCategoryNameAsync(dto.CategoryName, id);
//            if (exists)
//                return new OperationResult { Success = false, Message = "Ya existe otra categoría con ese nombre." };

//            var category = _mapper.Map<Categories>(dto);
//            return await _repository.UpdateAsync(category);
//        }
//    }
//}
