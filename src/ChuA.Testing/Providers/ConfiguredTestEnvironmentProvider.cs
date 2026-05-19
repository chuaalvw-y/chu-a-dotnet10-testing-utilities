using ChuA.Testing.Abstractions;
using ChuA.Testing.Configuration;
using Microsoft.Extensions.Options;

namespace ChuA.Testing.Providers;

/// <summary>
/// Provides test environment metadata from configuration.
/// </summary>
public sealed class ConfiguredTestEnvironmentProvider : ITestEnvironmentProvider
{
    private readonly IOptions<ChuATestingOptions> options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfiguredTestEnvironmentProvider"/> class.
    /// </summary>
    /// <param name="options">The testing options.</param>
    public ConfiguredTestEnvironmentProvider(IOptions<ChuATestingOptions> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public string EnvironmentName => options.Value.EnvironmentName;

    /// <inheritdoc />
    public bool IsTesting => EnvironmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase)
        || EnvironmentName.Equals("Test", StringComparison.OrdinalIgnoreCase)
        || EnvironmentName.EndsWith(".Testing", StringComparison.OrdinalIgnoreCase);
}
