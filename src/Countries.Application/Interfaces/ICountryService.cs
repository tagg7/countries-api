using Countries.Domain.Entities;

namespace Countries.Application.Interfaces;

public interface ICountryService
{
    Task<Country> Get(string id, CancellationToken cancellationToken = default);

    Task<ICollection<Country>> GetAll(CancellationToken cancellationToken = default);

    Task<Country> Update(Country country, CancellationToken cancellationToken = default);
}