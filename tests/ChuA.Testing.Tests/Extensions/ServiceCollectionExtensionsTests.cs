// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using ChuA.Testing.Abstractions;
using ChuA.Testing.Configuration;
using ChuA.Testing.Extensions;
using ChuA.Testing.Handlers;
using ChuA.Testing.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChuA.Testing.Tests.Extensions;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddChuATesting_RegistersExpectedServices()
    {
        using var provider = new ServiceCollection()
            .AddChuATesting(new ConfigurationBuilder().Build())
            .BuildServiceProvider(validateScopes: true);

        Assert.IsType<SystemTestClock>(provider.GetRequiredService<ITestClock>());
        Assert.IsType<DeterministicTestIdProvider>(provider.GetRequiredService<ITestIdProvider>());
        Assert.IsType<ConfiguredTestEnvironmentProvider>(provider.GetRequiredService<ITestEnvironmentProvider>());
        Assert.NotNull(provider.GetRequiredService<FakeHttpMessageHandler>());
        Assert.NotNull(provider.GetRequiredService<IOptions<ChuATestingOptions>>());
    }

    [Fact]
    public void AddChuATesting_DoesNotReplaceExistingAbstractions()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ITestClock, CustomClock>();

        using var provider = services
            .AddChuATesting(new ConfigurationBuilder().Build())
            .BuildServiceProvider(validateScopes: true);

        Assert.IsType<CustomClock>(provider.GetRequiredService<ITestClock>());
    }

    [Fact]
    public void AddChuATesting_ThrowsForMissingArguments()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddChuATesting(null!, configuration));
        Assert.Throws<ArgumentNullException>(() => services.AddChuATesting(null!));
        Assert.Throws<ArgumentException>(() => services.AddChuATesting(configuration, " "));
    }

    private sealed class CustomClock : ITestClock
    {
        public DateTimeOffset UtcNow => new(2030, 1, 1, 0, 0, 0, TimeSpan.Zero);
    }
}
