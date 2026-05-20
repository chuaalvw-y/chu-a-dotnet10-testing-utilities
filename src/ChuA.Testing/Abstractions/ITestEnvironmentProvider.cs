// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

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
