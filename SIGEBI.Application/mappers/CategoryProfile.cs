using AutoMapper;
using SIGEBI.Application.Dtos.CategoryDto;
using SIGEBI.Domain.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.mappers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryDTO, Categories>();
            CreateMap<UpdateCategoryDTO, Categories>();
            CreateMap<Categories, CategoryDTO>();
        }
    }
}
