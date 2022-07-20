using Countries.Domain.Common;

namespace Countries.Application.Interfaces;

public interface ICache<T> where T : EntityBase
{
    Task<(bool exists, T? item)> TryGet(string key);

    Task Set(string key, T item);
}