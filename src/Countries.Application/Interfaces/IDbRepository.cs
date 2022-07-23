using Countries.Domain.Common;

namespace Countries.Application.Interfaces;

public interface IDbRepository<T> where T : EntityBase
{
    Task<T> Get(string id, CancellationToken cancellationToken = default);

    Task<IList<T>> GetAll(CancellationToken cancellationToken = default);

    Task<T> Insert(T country, CancellationToken cancellationToken = default);
    
    Task<IList<T>> InsertMany(IEnumerable<T> countries, CancellationToken cancellationToken = default);

    Task<T> Update(T item, CancellationToken cancellationToken = default);
}