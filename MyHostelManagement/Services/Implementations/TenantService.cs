using AutoMapper;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.Api.DTOs;

namespace MyHostelManagement.Api.Services.Implementations;

public class TenantService : ITenantService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public TenantService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Tenant> AddTenantAsync(TenantDto dto)
    {
        var tenant = _mapper.Map<Tenant>(dto);
        
        var bed = await _uow.Beds.GetAsync(dto.BedId);
        if (bed == null) throw new Exception("Bed not found");
        bed.Status = "occupied";
        _uow.Beds.Update(bed);
        tenant.BedId = bed.Id;

        await _uow.Tenants.AddAsync(tenant);
        await _uow.SaveChangesAsync();
        return tenant;
    }

    public async Task<Tenant?> GetByIdAsync(Guid id) => await _uow.Tenants.GetAsync(id);

    public async Task<IEnumerable<Tenant>> GetByHostelAsync(Guid hostelId) =>
        (await _uow.Tenants.FindAsync(t => ((Tenant)t).HostelId == hostelId)).ToList();

    public async Task MoveAsync(Guid tenantId, Guid newRoomId, Guid newBedId)
    {
        var tenant = await _uow.Tenants.GetAsync(tenantId);
        if (tenant == null) throw new Exception("Tenant not found");

        // vacate old bed if present
        if (tenant.BedId.HasValue)
        {
            var oldBed = await _uow.Beds.GetAsync(tenant.BedId.Value);
            if (oldBed != null) { oldBed.Status = "available"; _uow.Beds.Update(oldBed); }
        }

        // assign new bed
        var newBed = await _uow.Beds.GetAsync(newBedId);
        if (newBed == null) throw new Exception("New bed not found");
        newBed.Status = "occupied";
        _uow.Beds.Update(newBed);

        tenant.RoomId = newRoomId;
        tenant.BedId = newBedId;
        _uow.Tenants.Update(tenant);

        await _uow.SaveChangesAsync();
    }

    public async Task VacateAsync(Guid tenantId)
    {
        var tenant = await _uow.Tenants.GetAsync(tenantId);
        if (tenant == null) throw new Exception("Tenant not found");
        if (tenant.BedId.HasValue)
        {
            var bed = await _uow.Beds.GetAsync(tenant.BedId.Value);
            if (bed != null) { bed.Status = "available"; _uow.Beds.Update(bed); }
        }
        tenant.CheckOut = DateTime.UtcNow;
        tenant.Status = "left";
        _uow.Tenants.Update(tenant);
        await _uow.SaveChangesAsync();
    }
}
