using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Infrastructure.Repositories;

public class Repository<T>: IRepository<T> where T : class
{
    public Task AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}
