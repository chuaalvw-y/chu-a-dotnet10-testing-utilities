// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.Testing.Models;

/// <summary>
/// Represents a stable identity value generated for a test scenario.
/// </summary>
/// <param name="Id">The generated identifier.</param>
/// <param name="Name">The generated display name.</param>
public sealed record TestIdentity(Guid Id, string Name);
