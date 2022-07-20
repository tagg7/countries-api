﻿using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace Countries.Web.Tests.Setup;

public class RedisFixture : IDisposable
{
    private readonly IContainerService _containerService;
    
    public RedisFixture()
    {
        _containerService = BuildContainerService();
    }

    private static IContainerService BuildContainerService() =>
        new Builder()
            .UseContainer()
            .UseImage("redis")
            .WithName("redis-integration-tests")
            .ExposePort(6480, 6479)
            .WaitForPort("6479/tcp", 30000)
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