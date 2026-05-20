// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using System.Net;
using ChuA.Testing.Configuration;
using ChuA.Testing.Handlers;
using Microsoft.Extensions.Options;

namespace ChuA.Testing.Tests.Handlers;

public sealed class FakeHttpMessageHandlerTests
{
    [Fact]
    public async Task SendAsync_ReturnsConfiguredResponse_ForMatchingRoute()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.Accepted)
        {
            Content = new StringContent("ok"),
        };
        using var handler = new FakeHttpMessageHandler(Options.Create(new ChuATestingOptions()))
            .When(HttpMethod.Get, "/health", response);
        using var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test"),
        };

        using var result = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
        Assert.Equal("ok", await result.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task SendAsync_Throws_ForUnmatchedRouteByDefault()
    {
        using var handler = new FakeHttpMessageHandler(Options.Create(new ChuATestingOptions()));
        using var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test"),
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetAsync("/missing"));

        Assert.Contains("GET /missing", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task SendAsync_ReturnsNotFound_ForUnmatchedRouteWhenConfigured()
    {
        using var handler = new FakeHttpMessageHandler(Options.Create(new ChuATestingOptions
        {
            ThrowOnUnmatchedHttpRequest = false,
        }));
        using var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://example.test"),
        };

        using var result = await client.GetAsync("/missing");

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
}
