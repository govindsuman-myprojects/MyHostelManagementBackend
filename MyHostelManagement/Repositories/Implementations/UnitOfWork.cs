using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.Repositories.Implementations;
using MyHostelManagement.Api.Repositories.Interfaces;

namespace MyHostelManagement.Api.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public IRepository<Models.Hostel> Hostels { get; private set; }
    public IRepository<Models.Room> Rooms { get; private set; }
    public IRepository<Models.Bed> Beds { get; private set; }
    public IRepository<Models.Tenant> Tenants { get; private set; }
    public IRepository<Models.Payment> Payments { get; private set; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Hostels = new GenericRepository<Models.Hostel>(_db);
        Rooms = new GenericRepository<Models.Room>(_db);
        Beds = new GenericRepository<Models.Bed>(_db);
        Tenants = new GenericRepository<Models.Tenant>(_db);
        Payments = new GenericRepository<Models.Payment>(_db);
    }

    public async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();

    public void Dispose() => _db.Dispose();
}
