using ChuA.Testing.Abstractions;
using ChuA.Testing.Configuration;
using ChuA.Testing.Constants;
using ChuA.Testing.Handlers;
using ChuA.Testing.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ChuA.Testing.Extensions;

/// <summary>
/// Provides dependency injection registration extensions for ChuA testing utilities.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds ChuA testing utilities using the default configuration section.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddChuATesting(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddChuATesting(configuration, ChuATestingDefaults.SectionName);
    }

    /// <summary>
    /// Adds ChuA testing utilities using a named configuration section.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <param name="sectionName">The configuration section name.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddChuATesting(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        services.AddOptions<ChuATestingOptions>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<ChuATestingOptions>, ChuATestingOptionsValidator>());
        services.TryAddSingleton<ITestClock, SystemTestClock>();
        services.TryAddSingleton<ITestIdProvider, DeterministicTestIdProvider>();
        services.TryAddSingleton<ITestEnvironmentProvider, ConfiguredTestEnvironmentProvider>();
        services.TryAddTransient<FakeHttpMessageHandler>();

        return services;
    }
}
