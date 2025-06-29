using AutoMapper;
using SIGEBI.Domain.Entities.circulation;
using SIGEBI.Application.Contracts.Services;
using SIGEBI.Application.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace SIGEBI.api.Services
{
    public class ReservationApiService
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReservationApiService(IReservationService reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationRequestDto requestDto)
        {
            var reservation = _mapper.Map<Reservation>(requestDto);
            // Fix: Replace the incorrect method call with the correct one based on the IReservationService interface
            var operationResult = await _reservationService.CreateReservationAsync(requestDto);

            if (!operationResult.IsSuccess)
            {
                throw new InvalidOperationException("Failed to create reservation.");
            }

            var createdReservation = _mapper.Map<Reservation>(operationResult.Data);
            var resultDto = _mapper.Map<ReservationDto>(createdReservation);
            return resultDto;
        }
    }
}
