using AutoMapper;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;

namespace MyHostelManagement.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PaymentFilterDto, Payment>();
        CreateMap<Room, RoomResponseDto>();
        // other maps as needed
    }
}
