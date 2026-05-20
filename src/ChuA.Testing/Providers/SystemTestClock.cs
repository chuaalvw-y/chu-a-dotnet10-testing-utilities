// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using ChuA.Testing.Abstractions;
using ChuA.Testing.Configuration;
using Microsoft.Extensions.Options;

namespace ChuA.Testing.Providers;

/// <summary>
/// Provides either deterministic or live UTC time based on <see cref="ChuATestingOptions"/>.
/// </summary>
public sealed class SystemTestClock : ITestClock
{
    private readonly IOptions<ChuATestingOptions> options;

    /// <summary>
    /// Initializes a new instance of the <see cref="SystemTestClock"/> class.
    /// </summary>
    /// <param name="options">The testing options.</param>
    public SystemTestClock(IOptions<ChuATestingOptions> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public DateTimeOffset UtcNow => options.Value.UseDeterministicValues
        ? options.Value.ClockUtcNow
        : DateTimeOffset.UtcNow;
}
