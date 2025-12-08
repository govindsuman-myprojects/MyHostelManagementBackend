using AutoMapper;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Repositories.Interfaces;

namespace MyHostelManagement.Api.Services.Implementations;

public class HostelService : IHostelService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IHostelRepository _hostelRepo;

    public HostelService(IUnitOfWork uow, IMapper mapper, IHostelRepository hostelRepo)
    {
        _uow = uow;
        _mapper = mapper;
        _hostelRepo = hostelRepo;
    }

    public async Task<Hostel> CreateAsync(HostelDto dto)
    {
        var hostel = _mapper.Map<Hostel>(dto);
        hostel.Id = Guid.NewGuid();

        await _uow.Hostels.AddAsync(hostel);
        await _uow.SaveChangesAsync();
        return hostel;
    }

    public async Task<IEnumerable<Hostel>> GetAllAsync() => await _uow.Hostels.GetAllAsync();

    public async Task<Hostel?> GetByIdAsync(Guid id) => await _uow.Hostels.GetAsync(id);

    public async Task<OwnerDashboardResponse> GetOwnerDashboardAsync(Guid id)
    {

        var reponse =  await _hostelRepo.GetOwnerDashboardAsync(id);


        return reponse; 
    }
}
