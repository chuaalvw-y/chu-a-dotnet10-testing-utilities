// Copyright (c) 2026 Alvin Wilsen Chan Chua
// GitHub: chuaalvw-y
// Licensed under the Alvin Wilsen Chan Chua Proprietary Use-Only License.
// See LICENSE.txt in the project root for full license information.

using ChuA.Testing.Services;
using ChuA.Testing.Utilities;

namespace ChuA.Testing.Tests.Utilities;

public sealed class TestDataBuilderTests
{
    [Fact]
    public void Build_AppliesCustomizers()
    {
        var customer = new TestDataBuilder<TestCustomer>(() => new TestCustomer())
            .With(value => value.Name = "Ada")
            .Build();

        Assert.Equal("Ada", customer.Name);
    }

    [Fact]
    public void Build_Throws_WhenFactoryReturnsNull()
    {
        var builder = new TestDataBuilder<TestCustomer?>(() => null);

        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }

    [Fact]
    public void ShouldNotBeNull_ReturnsValue_WhenPresent()
    {
        var value = AssertionUtilities.ShouldNotBeNull("value");

        Assert.Equal("value", value);
    }

    [Fact]
    public async Task ShouldCompleteWithinAsync_Throws_WhenTimeoutExpires()
    {
        await Assert.ThrowsAsync<TimeoutException>(() =>
            AssertionUtilities.ShouldCompleteWithinAsync(
                async cancellationToken => await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken),
                TimeSpan.FromMilliseconds(10)));
    }

    private sealed class TestCustomer
    {
        public string? Name { get; set; }
    }
}
