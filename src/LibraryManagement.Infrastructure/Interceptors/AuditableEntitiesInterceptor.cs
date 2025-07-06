using LibraryManagement.Domain.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace LibraryManagement.Infrastructure.Interceptors;

public sealed class AuditableEntitiesInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    private readonly HttpContext? _httpContext = httpContextAccessor.HttpContext;
    private Guid? GetCurrentUserId()
    {
        var id = _httpContext?.User?.Claims.FirstOrDefault(claim => claim.Type is ClaimTypes.NameIdentifier)?.Value;
        return id is null ? null : new Guid(id);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellation = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellation);
        }

        IEnumerable<EntityEntry<IAuditableEntity>> entries = dbContext.ChangeTracker.Entries<IAuditableEntity>();

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

        return base.SavingChangesAsync(eventData, result, cancellation);
    }
}
