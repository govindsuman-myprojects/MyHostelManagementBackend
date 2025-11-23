using System.Linq.Expressions;

namespace MyHostelManagement.Api.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
