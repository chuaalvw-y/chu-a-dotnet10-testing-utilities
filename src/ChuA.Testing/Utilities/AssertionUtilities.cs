// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.Testing.Utilities;

/// <summary>
/// Provides framework-neutral assertion helpers for shared test libraries.
/// </summary>
public static class AssertionUtilities
{
    /// <summary>
    /// Throws when the provided value is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    /// <param name="value">The value to inspect.</param>
    /// <param name="message">The exception message.</param>
    /// <returns>The non-null value.</returns>
    public static T ShouldNotBeNull<T>(T? value, string? message = null)
        where T : class
    {
        return value ?? throw new InvalidOperationException(message ?? "Expected a non-null value.");
    }

    /// <summary>
    /// Throws when the action does not complete within the expected timeout.
    /// </summary>
    /// <param name="action">The asynchronous action to execute.</param>
    /// <param name="timeout">The timeout.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task ShouldCompleteWithinAsync(
        Func<CancellationToken, Task> action,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(action);

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout must be greater than zero.");
        }

        using var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var operation = action(timeoutSource.Token);
        var delay = Task.Delay(timeout, timeoutSource.Token);

        if (await Task.WhenAny(operation, delay).ConfigureAwait(false) != operation)
        {
            throw new TimeoutException($"Expected the operation to complete within {timeout}.");
        }

        await timeoutSource.CancelAsync().ConfigureAwait(false);
        await operation.ConfigureAwait(false);
    }
}
