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
