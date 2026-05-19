namespace ChuA.Testing.Abstractions;

/// <summary>
/// Provides environment metadata for tests.
/// </summary>
public interface ITestEnvironmentProvider
{
    /// <summary>
    /// Gets the configured test environment name.
    /// </summary>
    string EnvironmentName { get; }

    /// <summary>
    /// Gets a value indicating whether the current environment should be treated as a test environment.
    /// </summary>
    bool IsTesting { get; }
}
