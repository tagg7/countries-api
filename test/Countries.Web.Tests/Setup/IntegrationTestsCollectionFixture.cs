using Xunit;

namespace Countries.Web.Tests.Setup;

[CollectionDefinition("IntegrationTestsCollection", DisableParallelization = true)]
public class IntegrationTestsCollectionFixture : ICollectionFixture<MongoDbFixture>, ICollectionFixture<RedisFixture>
{
}