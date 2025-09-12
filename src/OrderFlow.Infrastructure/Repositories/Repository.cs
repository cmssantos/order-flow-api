using Microsoft.EntityFrameworkCore;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Infrastructure.Repositories;

public class Repository<T>(DbContext context): IRepository<T> where T : class
{
    private readonly DbContext context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly DbSet<T> dbSet = context.Set<T>();

     public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbSet.FindAsync([id], cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await dbSet.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        dbSet.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        dbSet.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
