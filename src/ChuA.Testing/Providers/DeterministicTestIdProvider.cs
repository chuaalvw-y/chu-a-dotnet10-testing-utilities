using System.Security.Cryptography;
using System.Text;
using ChuA.Testing.Abstractions;
using ChuA.Testing.Configuration;
using ChuA.Testing.Models;
using Microsoft.Extensions.Options;

namespace ChuA.Testing.Providers;

/// <summary>
/// Generates deterministic or random test identities based on <see cref="ChuATestingOptions"/>.
/// </summary>
public sealed class DeterministicTestIdProvider : ITestIdProvider
{
    private readonly IOptions<ChuATestingOptions> options;
    private int sequence;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicTestIdProvider"/> class.
    /// </summary>
    /// <param name="options">The testing options.</param>
    public DeterministicTestIdProvider(IOptions<ChuATestingOptions> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
        sequence = options.Value.Seed;
    }

    /// <inheritdoc />
    public TestIdentity Next(string? prefix = null)
    {
        var safePrefix = string.IsNullOrWhiteSpace(prefix) ? "test" : prefix.Trim();
        var next = Interlocked.Increment(ref sequence);
        var id = options.Value.UseDeterministicValues
            ? CreateDeterministicGuid($"{options.Value.Seed}:{safePrefix}:{next}")
            : Guid.NewGuid();

        return new TestIdentity(id, $"{safePrefix}-{next}");
    }

    private static Guid CreateDeterministicGuid(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return new Guid(bytes[..16]);
    }
}
