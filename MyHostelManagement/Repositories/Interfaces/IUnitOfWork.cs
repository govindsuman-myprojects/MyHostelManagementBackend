using MyHostelManagement.Api.Repositories.Interfaces;

namespace MyHostelManagement.Api.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Models.Hostel> Hostels { get; }
    IRepository<Models.Room> Rooms { get; }
    IRepository<Models.Bed> Beds { get; }
    IRepository<Models.Tenant> Tenants { get; }
    IRepository<Models.Payment> Payments { get; }
    // add other repos if needed (MessRecords, Maintenance, etc.)

    Task<int> SaveChangesAsync();
}
