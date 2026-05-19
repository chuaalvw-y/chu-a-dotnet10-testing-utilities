namespace ChuA.Testing.Constants;

/// <summary>
/// Provides default values used by the ChuA testing utilities.
/// </summary>
public static class ChuATestingDefaults
{
    /// <summary>
    /// Gets the default configuration section name.
    /// </summary>
    public const string SectionName = "ChuA:Testing";

    /// <summary>
    /// Gets the default deterministic seed.
    /// </summary>
    public const int Seed = 10_000;

    /// <summary>
    /// Gets the default timeout for test infrastructure operations.
    /// </summary>
    public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets the default fixed UTC clock value.
    /// </summary>
    public static readonly DateTimeOffset ClockUtcNow = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
}
