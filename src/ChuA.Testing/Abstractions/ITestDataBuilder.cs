// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.Testing.Abstractions;

/// <summary>
/// Builds configured test data instances.
/// </summary>
/// <typeparam name="T">The model type to build.</typeparam>
public interface ITestDataBuilder<out T>
{
    /// <summary>
    /// Builds a new instance.
    /// </summary>
    /// <returns>The configured instance.</returns>
    T Build();
}
