using System.Linq.Expressions;

namespace LibraryManagement.Domain.Abstracts;

public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetListAsync();
    Task<TEntity?> GetAsync(Guid id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
