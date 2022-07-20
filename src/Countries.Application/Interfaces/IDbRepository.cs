using Countries.Domain.Common;

namespace Countries.Application.Interfaces;

public interface IDbRepository<T> where T : EntityBase
{
    Task<T> Get(string id, CancellationToken cancellationToken = default);

    Task<ICollection<T>> GetAll(CancellationToken cancellationToken = default);

    Task<T> Update(T item, CancellationToken cancellationToken = default);
}