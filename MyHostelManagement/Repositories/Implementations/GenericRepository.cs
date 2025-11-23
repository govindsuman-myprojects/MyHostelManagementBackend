using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.Repositories.Interfaces;
using System.Linq.Expressions;

namespace MyHostelManagement.Api.Repositories.Implementations;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _db;
    protected readonly DbSet<TEntity> _set;
    public GenericRepository(ApplicationDbContext db)
    {
        _db = db;
        _set = _db.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity) => await _set.AddAsync(entity);

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _set.Where(predicate).ToListAsync();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await _set.ToListAsync();

    public virtual async Task<TEntity?> GetAsync(Guid id) => await _set.FindAsync(id);

    public virtual void Remove(TEntity entity) => _set.Remove(entity);

    public virtual void Update(TEntity entity) => _set.Update(entity);
}
