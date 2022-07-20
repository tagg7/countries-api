using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace Countries.Web.Tests.Setup;

public class MongoDbFixture : IDisposable
{
    private readonly IContainerService _containerService;

    public MongoDbFixture()
    {
        _containerService = BuildContainerService();
    }

    private static IContainerService BuildContainerService() =>
        new Builder()
            .UseContainer()
            .UseImage("mongo")
            .WithName("mongo-integration-tests")
            .ExposePort(27018, 27017)
            .WaitForPort("27017/tcp", 30000)
            .RemoveVolumesOnDispose(true)
            .DeleteIfExists(force: true)
            .Build()
            .Start();

    public void Dispose()
    {
        _containerService?.Dispose();
        GC.SuppressFinalize(this);
    }
}