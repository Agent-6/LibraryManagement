using LibraryManagement.Application.Persistance;
using LibraryManagement.Domain.Abstracts;
using LibraryManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

namespace LibraryManagement.Infrastructure.Persistance;

internal sealed class UnitOfWork(LibraryManagementDbContext dbContext, IHttpContextAccessor httpContextAccessor) : IUnitOfWork
{
    private readonly LibraryManagementDbContext _dbContext = dbContext;
    private readonly HttpContext? _httpContext = httpContextAccessor.HttpContext;
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries = _dbContext.ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State is EntityState.Added)
            {
                entityEntry.Property(e => e.CreatorId).CurrentValue = GetCurrentUserId() ?? Guid.Empty;
                entityEntry.Property(e => e.CreationTime).CurrentValue = DateTime.UtcNow;
            }

            if (entityEntry.State is EntityState.Modified)
            {
                entityEntry.Property(e => e.ModifierId).CurrentValue = GetCurrentUserId();
                entityEntry.Property(e => e.ModifiedTime).CurrentValue = DateTime.UtcNow;
            }
        }
    }

    private Guid? GetCurrentUserId()
    {
        var id = _httpContext?.User?.Claims.FirstOrDefault(claim => claim.Type is ClaimTypes.NameIdentifier)?.Value;
        return id is null ? null : new Guid(id);
    }
}
