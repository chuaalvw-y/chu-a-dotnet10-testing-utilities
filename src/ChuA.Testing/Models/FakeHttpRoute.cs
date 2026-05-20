// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

namespace ChuA.Testing.Models;

/// <summary>
/// Describes a route handled by the fake HTTP message handler.
/// </summary>
/// <param name="Method">The HTTP method to match.</param>
/// <param name="Path">The absolute path to match.</param>
/// <param name="ResponseFactory">The response factory executed for a matching request.</param>
public sealed record FakeHttpRoute(
    HttpMethod Method,
    string Path,
    Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> ResponseFactory);
