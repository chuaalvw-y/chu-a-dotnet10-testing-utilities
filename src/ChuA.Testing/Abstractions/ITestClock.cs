namespace ChuA.Testing.Abstractions;

/// <summary>
/// Provides the current time for tests and test infrastructure.
/// </summary>
public interface ITestClock
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
