// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using ChuA.Testing.Configuration;
using ChuA.Testing.Providers;
using Microsoft.Extensions.Options;

namespace ChuA.Testing.Tests.Providers;

public sealed class CoreProviderTests
{
    [Fact]
    public void SystemTestClock_ReturnsConfiguredClock_WhenDeterministic()
    {
        var expected = new DateTimeOffset(2026, 5, 17, 1, 2, 3, TimeSpan.Zero);
        var clock = new SystemTestClock(Options.Create(new ChuATestingOptions
        {
            UseDeterministicValues = true,
            ClockUtcNow = expected,
        }));

        Assert.Equal(expected, clock.UtcNow);
    }

    [Fact]
    public void DeterministicTestIdProvider_ReturnsRepeatableIds()
    {
        var options = Options.Create(new ChuATestingOptions
        {
            Seed = 7,
            UseDeterministicValues = true,
        });

        var first = new DeterministicTestIdProvider(options).Next("customer");
        var second = new DeterministicTestIdProvider(options).Next("customer");

        Assert.Equal(first, second);
        Assert.Equal("customer-8", first.Name);
    }

    [Theory]
    [InlineData("Testing", true)]
    [InlineData("Test", true)]
    [InlineData("Payments.Testing", true)]
    [InlineData("Production", false)]
    public void ConfiguredTestEnvironmentProvider_DetectsTestingEnvironment(string environmentName, bool expected)
    {
        var provider = new ConfiguredTestEnvironmentProvider(Options.Create(new ChuATestingOptions
        {
            EnvironmentName = environmentName,
        }));

        Assert.Equal(expected, provider.IsTesting);
    }
}
