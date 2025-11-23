using AutoMapper;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Api.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<HostelDto, Hostel>().ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<RoomDto, Room>();
        CreateMap<TenantDto, Tenant>();
        CreateMap<PaymentDto, Payment>();
        // other maps as needed
    }
}
