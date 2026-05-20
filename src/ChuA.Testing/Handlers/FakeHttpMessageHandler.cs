// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using ChuA.Testing.Configuration;
using ChuA.Testing.Models;
using Microsoft.Extensions.Options;

namespace ChuA.Testing.Handlers;

/// <summary>
/// Provides a reusable fake <see cref="HttpMessageHandler"/> for unit and integration-style tests.
/// </summary>
public sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly List<FakeHttpRoute> routes = [];
    private readonly IOptions<ChuATestingOptions> options;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeHttpMessageHandler"/> class.
    /// </summary>
    /// <param name="options">The testing options.</param>
    public FakeHttpMessageHandler(IOptions<ChuATestingOptions> options)
    {
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Adds a route that returns a static response.
    /// </summary>
    /// <param name="method">The HTTP method to match.</param>
    /// <param name="path">The absolute path to match.</param>
    /// <param name="response">The response to return.</param>
    /// <returns>The current handler.</returns>
    public FakeHttpMessageHandler When(HttpMethod method, string path, HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);
        return When(method, path, (_, _) => Task.FromResult(response));
    }

    /// <summary>
    /// Adds a route that returns a dynamic response.
    /// </summary>
    /// <param name="method">The HTTP method to match.</param>
    /// <param name="path">The absolute path to match.</param>
    /// <param name="responseFactory">The response factory.</param>
    /// <returns>The current handler.</returns>
    public FakeHttpMessageHandler When(
        HttpMethod method,
        string path,
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responseFactory)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        ArgumentNullException.ThrowIfNull(responseFactory);

        routes.Add(new FakeHttpRoute(method, NormalizePath(path), responseFactory));
        return this;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestPath = NormalizePath(request.RequestUri?.AbsolutePath ?? "/");
        var route = routes.LastOrDefault(candidate =>
            candidate.Method == request.Method &&
            candidate.Path.Equals(requestPath, StringComparison.OrdinalIgnoreCase));

        if (route is not null)
        {
            return await route.ResponseFactory(request, cancellationToken).ConfigureAwait(false);
        }

        if (options.Value.ThrowOnUnmatchedHttpRequest)
        {
            throw new InvalidOperationException($"No fake HTTP route matched {request.Method} {requestPath}.");
        }

        return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
        {
            RequestMessage = request,
        };
    }

    private static string NormalizePath(string path)
    {
        var normalized = path.Trim();
        return normalized.StartsWith("/", StringComparison.Ordinal) ? normalized : $"/{normalized}";
    }
}
