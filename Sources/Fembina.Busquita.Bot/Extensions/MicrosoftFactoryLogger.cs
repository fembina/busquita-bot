using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Fembina.Busquita.Bot.Extensions;

public sealed class MicrosoftFactoryLogger<T> : ILogger<T>
{
    private readonly ILogger _logger;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MicrosoftFactoryLogger(ILoggerFactory loggerFactory)
    {
        var name = typeof(T).FullName;

        ArgumentException.ThrowIfNullOrEmpty(name);

        _logger = loggerFactory.CreateLogger(name);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _logger.BeginScope(state);
    }
}
