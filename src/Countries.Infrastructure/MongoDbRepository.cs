using Countries.Application.Interfaces;
using Countries.Domain.Entities;
using MongoDB.Driver;

namespace Countries.Infrastructure;

public class MongoDbRepository : IDbRepository<Country>
{
    private readonly IMongoCollection<Country> _collection;

    public MongoDbRepository(IMongoDatabase mongoDatabase)
    {
        // TODO: Should get the database name from configuration
        _collection = mongoDatabase.GetCollection<Country>("Countries");
    }

    public async Task<Country> Get(string id, CancellationToken cancellationToken = default)
    {
        var cursor = await _collection.FindAsync(c => c.Id == id, cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<IList<Country>> GetAll(CancellationToken cancellationToken = default)
    {
        // TODO: Should use paging
        var cursor = await _collection.FindAsync(_ => true, cancellationToken: cancellationToken);
        return await cursor.ToListAsync(cancellationToken: cancellationToken);
    }
    
    public async Task<Country> Insert(Country country, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(country, cancellationToken: cancellationToken);
        return country;
    }

    public async Task<IList<Country>> InsertMany(IEnumerable<Country> countries, CancellationToken cancellationToken = default)
    {
        await _collection.InsertManyAsync(countries, cancellationToken: cancellationToken);
        return countries.ToList();
    }

    public async Task<Country> Update(Country country, CancellationToken cancellationToken = default)
    {
        var options = new FindOneAndReplaceOptions<Country> { IsUpsert = true };
        return await _collection.FindOneAndReplaceAsync<Country>(c => c.Id == country.Id, country, options, cancellationToken);
    }
}