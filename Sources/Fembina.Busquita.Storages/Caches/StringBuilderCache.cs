using System.Text;

namespace Fembina.Busquita.Storages.Caches;

/// <summary>
/// A cache for <see cref="StringBuilder"/> instances.
/// This class is designed to improve performance by reusing StringBuilder instances.
/// This class is thread-safe.
/// Caches a single StringBuilder instance or creates a new one if the cached instance is not available.
/// </summary>
public sealed class StringBuilderCache
{
    // I'm caching the TrySet delegate to avoid creating a new delegate instance every time
    private readonly Action<StringBuilder> _trySetDelegate;

    private readonly int _capacity;

    /// I'm caching the last set StringBuilder instance to avoid creating a new one every time
    private StringBuilder? _builder;

    /// <summary>
    /// Creates a new instance of the <see cref="StringBuilderCache"/> class with the specified capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the StringBuilder.</param>
    public StringBuilderCache(int capacity = 256)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity, nameof(capacity));

        _capacity = capacity;
        _trySetDelegate = TrySet;
    }

    /// <summary>
    /// Gets or creates a <see cref="ShotTimeValue{StringBuilder}"/> instance.
    /// </summary>
    /// <returns>
    /// Returns a <see cref="ShotTimeValue{StringBuilder}"/> instance
    /// containing a StringBuilder with the specified capacity.
    /// </returns>
    /// <remarks>You should dispose of returned ShotTimeValue when you're done with it.</remarks>
    public ShotTimeValue<StringBuilder> GetOrCreate()
    {
        var builder = Interlocked.Exchange(ref _builder, null);

        if (builder is null) return new ShotTimeValue<StringBuilder>(new StringBuilder(_capacity), _trySetDelegate);

        if (builder.Length > 0) builder.Clear();

        if (builder.Capacity > _capacity) builder.EnsureCapacity(_capacity);

        return new ShotTimeValue<StringBuilder>(builder, _trySetDelegate);
    }

    private void TrySet(StringBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Interlocked.CompareExchange(ref _builder, builder, null);
    }
}
