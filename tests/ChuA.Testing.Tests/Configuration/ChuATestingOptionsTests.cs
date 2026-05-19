using ChuA.Testing.Configuration;
using ChuA.Testing.Constants;
using ChuA.Testing.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ChuA.Testing.Tests.Configuration;

public sealed class ChuATestingOptionsTests
{
    [Fact]
    public void AddChuATesting_BindsDefaultSection()
    {
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["ChuA:Testing:UseDeterministicValues"] = "false",
            ["ChuA:Testing:Seed"] = "42",
            ["ChuA:Testing:ClockUtcNow"] = "2026-05-17T12:00:00+00:00",
            ["ChuA:Testing:DefaultTimeout"] = "00:00:05",
            ["ChuA:Testing:ThrowOnUnmatchedHttpRequest"] = "false",
            ["ChuA:Testing:EnvironmentName"] = "Integration.Testing",
        });

        using var provider = new ServiceCollection()
            .AddChuATesting(configuration)
            .BuildServiceProvider(validateScopes: true);

        var options = provider.GetRequiredService<IOptions<ChuATestingOptions>>().Value;

        Assert.False(options.UseDeterministicValues);
        Assert.Equal(42, options.Seed);
        Assert.Equal(TimeSpan.FromSeconds(5), options.DefaultTimeout);
        Assert.False(options.ThrowOnUnmatchedHttpRequest);
        Assert.Equal("Integration.Testing", options.EnvironmentName);
    }

    [Fact]
    public void AddChuATesting_BindsCustomSection()
    {
        var configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["TestingUtilities:Seed"] = "123",
            ["TestingUtilities:EnvironmentName"] = "Testing",
        });

        using var provider = new ServiceCollection()
            .AddChuATesting(configuration, "TestingUtilities")
            .BuildServiceProvider(validateScopes: true);

        var options = provider.GetRequiredService<IOptions<ChuATestingOptions>>().Value;

        Assert.Equal(123, options.Seed);
        Assert.Equal("Testing", options.EnvironmentName);
    }

    [Fact]
    public void AddChuATesting_UsesSecureDefaults_WhenSectionMissing()
    {
        using var provider = new ServiceCollection()
            .AddChuATesting(CreateConfiguration())
            .BuildServiceProvider(validateScopes: true);

        var options = provider.GetRequiredService<IOptions<ChuATestingOptions>>().Value;

        Assert.True(options.UseDeterministicValues);
        Assert.Equal(ChuATestingDefaults.Seed, options.Seed);
        Assert.True(options.ThrowOnUnmatchedHttpRequest);
        Assert.Equal("Testing", options.EnvironmentName);
    }

    [Fact]
    public void OptionsValidator_FailsForInvalidValues()
    {
        var validator = new ChuATestingOptionsValidator();

        var result = validator.Validate(null, new ChuATestingOptions
        {
            Seed = 0,
            DefaultTimeout = TimeSpan.Zero,
            EnvironmentName = " ",
        });

        Assert.True(result.Failed);
        Assert.Contains(result.Failures, failure => failure.Contains(nameof(ChuATestingOptions.Seed), StringComparison.Ordinal));
        Assert.Contains(result.Failures, failure => failure.Contains(nameof(ChuATestingOptions.DefaultTimeout), StringComparison.Ordinal));
        Assert.Contains(result.Failures, failure => failure.Contains(nameof(ChuATestingOptions.EnvironmentName), StringComparison.Ordinal));
    }

    private static IConfiguration CreateConfiguration(Dictionary<string, string?>? values = null)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values ?? new Dictionary<string, string?>())
            .Build();
    }
}
