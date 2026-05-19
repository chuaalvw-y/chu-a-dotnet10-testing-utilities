using ChuA.Testing.Abstractions;

namespace ChuA.Testing.Services;

/// <summary>
/// Builds test data through a composable factory and customizer pipeline.
/// </summary>
/// <typeparam name="T">The type to build.</typeparam>
public sealed class TestDataBuilder<T> : ITestDataBuilder<T>
{
    private readonly Func<T> factory;
    private readonly List<Action<T>> customizers = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="TestDataBuilder{T}"/> class.
    /// </summary>
    /// <param name="factory">The factory used to create the instance.</param>
    public TestDataBuilder(Func<T> factory)
    {
        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// Adds a customizer that runs before the instance is returned.
    /// </summary>
    /// <param name="customizer">The customizer to apply.</param>
    /// <returns>The current builder.</returns>
    public TestDataBuilder<T> With(Action<T> customizer)
    {
        customizers.Add(customizer ?? throw new ArgumentNullException(nameof(customizer)));
        return this;
    }

    /// <inheritdoc />
    public T Build()
    {
        var instance = factory();
        if (instance is null)
        {
            throw new InvalidOperationException("The test data factory returned null.");
        }

        foreach (var customizer in customizers)
        {
            customizer(instance);
        }

        return instance;
    }
}
