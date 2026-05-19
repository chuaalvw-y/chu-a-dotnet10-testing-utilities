namespace ChuA.Testing.Models;

/// <summary>
/// Represents a stable identity value generated for a test scenario.
/// </summary>
/// <param name="Id">The generated identifier.</param>
/// <param name="Name">The generated display name.</param>
public sealed record TestIdentity(Guid Id, string Name);
