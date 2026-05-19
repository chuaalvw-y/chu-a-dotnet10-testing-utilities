using ChuA.Testing.Models;

namespace ChuA.Testing.Abstractions;

/// <summary>
/// Generates identifiers suitable for repeatable tests.
/// </summary>
public interface ITestIdProvider
{
    /// <summary>
    /// Creates the next test identity.
    /// </summary>
    /// <param name="prefix">The optional name prefix.</param>
    /// <returns>A generated test identity.</returns>
    TestIdentity Next(string? prefix = null);
}
