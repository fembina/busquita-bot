namespace Fembina.Busquita.Storages.Caches;

public readonly ref struct ShotTimeValue<T>(T value, Action<T> dispose) : IDisposable
{
    public readonly T Value = value;

    public void Dispose() => dispose(Value);

    public static implicit operator T(ShotTimeValue<T> value) => value.Value;
}
