using Countries.Application.Interfaces;
using Countries.Domain.Entities;

namespace Countries.Infrastructure;

public class CountryService : ICountryService
{
    private readonly IDbRepository<Country> _dbRepository;
    private readonly ICache<Country> _cache;

    public CountryService(IDbRepository<Country> dbRepository, ICache<Country> cache)
    {
        _dbRepository = dbRepository;
        _cache = cache;
    }

    public async Task<Country> Get(string id, CancellationToken cancellationToken = default)
    {
        var (exists, country) = await _cache.TryGet(id);
        if (exists)
            return country!;

        country = await _dbRepository.Get(id, cancellationToken);
        await _cache.Set(id, country);
        return country;
    }

    public async Task<IList<Country>> GetAll(CancellationToken cancellationToken = default)
    {
        var countries = await _dbRepository.GetAll(cancellationToken);
        return countries;
    }

    public async Task<Country> Update(Country country, CancellationToken cancellationToken = default)
    {
        country = await _dbRepository.Update(country, cancellationToken);
        await _cache.Set(country.Id, country);
        return country;
    }
}