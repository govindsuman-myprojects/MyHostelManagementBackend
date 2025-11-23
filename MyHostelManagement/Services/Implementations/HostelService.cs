using AutoMapper;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.Api.Services.Interfaces;

namespace MyHostelManagement.Api.Services.Implementations;

public class HostelService : IHostelService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public HostelService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
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
}
