using AutoMapper;
using SIGEBI.Application.DTOs;
using SIGEBI.Domain.Entities.circulation;

namespace SIGEBI.api.Mapping
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<CreateReservationRequestDto, Reservation>() //Crear reserva
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.StatusId, opt => opt.Ignore());

            CreateMap<Reservation, ReservationDto>() //devolver reserva
                //.ForMember(dest => dest.ReservationStatus, opt => opt.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId));

            CreateMap<ReservationStatus, ReservationStatusDto>(); //devolver estado

            //CreateMap<ReservationHistory, ReservationHistoryDto>() //devolver historial de reservas
                //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusId));
                //.ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.ReservationDate))
                //.ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
                //.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                //.ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId));
        }
    }
}
