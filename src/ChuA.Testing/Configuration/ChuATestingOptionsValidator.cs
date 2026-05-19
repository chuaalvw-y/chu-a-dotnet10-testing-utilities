using Microsoft.Extensions.Options;

namespace ChuA.Testing.Configuration;

/// <summary>
/// Validates <see cref="ChuATestingOptions"/> for safe runtime behavior.
/// </summary>
public sealed class ChuATestingOptionsValidator : IValidateOptions<ChuATestingOptions>
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, ChuATestingOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var failures = new List<string>();

        if (options.Seed <= 0)
        {
            failures.Add($"{nameof(ChuATestingOptions.Seed)} must be greater than zero.");
        }

        if (options.DefaultTimeout <= TimeSpan.Zero)
        {
            failures.Add($"{nameof(ChuATestingOptions.DefaultTimeout)} must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(options.EnvironmentName))
        {
            failures.Add($"{nameof(ChuATestingOptions.EnvironmentName)} is required.");
        }

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }
}
