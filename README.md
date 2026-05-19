# ChuA.Testing

ChuA.Testing is an enterprise-grade reusable testing utilities library for .NET 10 applications. It provides strongly typed configuration, dependency injection registration, deterministic providers, fakes, builders, and lightweight assertion helpers for ASP.NET Core Web API, MVC, Razor Pages, Blazor Server, worker services, and internal enterprise applications.

## Architecture Overview

The library is intentionally small, interface-first, and friendly to unit tests.

- `Abstractions`: reusable contracts such as `ITestClock`, `ITestIdProvider`, `ITestEnvironmentProvider`, and `ITestDataBuilder<T>`.
- `Configuration`: strongly typed `ChuATestingOptions` plus explicit validation.
- `Extensions`: single-call DI startup through `AddChuATesting`.
- `Providers`: default implementations for clocks, deterministic identities, and environment metadata.
- `Handlers`: `FakeHttpMessageHandler` for safe HTTP client tests.
- `Services`: composable test data builders.
- `Utilities`: framework-neutral assertion helpers.

Defaults are deterministic and fail safely. Unmatched fake HTTP requests throw by default so tests do not accidentally pass after calling an unexpected endpoint.

## Setup

Target framework:

```xml
<TargetFramework>net10.0</TargetFramework>
```

Register the library with the default configuration section:

```csharp
using ChuA.Testing.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddChuATesting(builder.Configuration);
```

Or use a custom section name:

```csharp
builder.Services.AddChuATesting(builder.Configuration, "TestingUtilities");
```

## Sample Configuration

```json
{
  "ChuA": {
    "Testing": {
      "UseDeterministicValues": true,
      "Seed": 10000,
      "ClockUtcNow": "2026-01-01T00:00:00+00:00",
      "DefaultTimeout": "00:00:30",
      "ThrowOnUnmatchedHttpRequest": true,
      "EnvironmentName": "Testing"
    }
  }
}
```

A package content sample is also included at `src/ChuA.Testing/appsettings.ChuA.Testing.sample.json`.

## Usage Examples

Resolve deterministic test infrastructure from DI:

```csharp
public sealed class CustomerWorkflowTests
{
    private readonly ITestClock clock;
    private readonly ITestIdProvider ids;

    public CustomerWorkflowTests(ITestClock clock, ITestIdProvider ids)
    {
        this.clock = clock;
        this.ids = ids;
    }

    [Fact]
    public void CreatesCustomerWithStableMetadata()
    {
        var identity = ids.Next("customer");

        Assert.Equal("customer-10001", identity.Name);
        Assert.Equal(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero), clock.UtcNow);
    }
}
```

Use the fake HTTP handler:

```csharp
var handler = new FakeHttpMessageHandler(options)
    .When(HttpMethod.Get, "/health", new HttpResponseMessage(HttpStatusCode.OK));

var client = new HttpClient(handler)
{
    BaseAddress = new Uri("https://internal-api.test")
};
```

Build test data with focused customizations:

```csharp
var customer = new TestDataBuilder<Customer>(() => new Customer())
    .With(value => value.Name = "Ada")
    .Build();
```

## Extension Points

Applications can replace any default abstraction before calling `AddChuATesting`; the library uses `TryAdd` registrations and preserves existing services.

Common future additions fit naturally into the current structure:

- database fixture abstractions
- authentication and authorization test identities
- fake message bus handlers
- test host builders for Web API, MVC, Razor Pages, Blazor Server, and worker services
- snapshot or contract assertion utilities

## Build And Test

```powershell
dotnet restore ChuA.Testing.slnx
dotnet build ChuA.Testing.slnx
dotnet test ChuA.Testing.slnx
```
