using LibraryManagement.Domain.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagement.Infrastructure.Data;

internal abstract class BaseRepository<TEntity>(LibraryManagementDbContext dbContext) : IRepository<TEntity> where TEntity : class
{
    protected DbSet<TEntity> DbSet { get; } = dbContext.Set<TEntity>();
    protected Task<int> SaveChangesAsync() => dbContext.SaveChangesAsync();

    public virtual async Task<List<TEntity>> GetListAsync() => await DbSet.ToListAsync();

    public virtual async Task<TEntity?> GetAsync(Guid id) => await DbSet.FindAsync(id);
    
    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
        await DbSet.Where(predicate).FirstOrDefaultAsync();

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await SaveChangesAsync();

        return entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChangesAsync();

        return entity;
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        await SaveChangesAsync();
    }
}
