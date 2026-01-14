using AutoMapper;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;

namespace MyHostelManagement.Api.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PaymentFilterDto, Payment>();
        CreateMap<Room, RoomResponseDto>();
        // other maps as needed
    }
}
