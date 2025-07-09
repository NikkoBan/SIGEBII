using AutoMapper;
using SIGEBI.Domain.Entities;
using SIGEBI.Application.DTOsAplication.UserDTOs;
using SIGEBI.Application.DTOsAplication.UserHistoryDTOs;

namespace SIGEBI.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // --- Mapeos para User ---
            CreateMap<User, UserDisplayDto>();
            CreateMap<UserCreationDto, User>();
            CreateMap<UserUpdateDto, User>();

            // --- Mapeos para UserHistory ---
            CreateMap<UserHistory, UserHistoryDisplayDto>();
            CreateMap<UserHistoryCreationDto, UserHistory>();
        }
    }
}
