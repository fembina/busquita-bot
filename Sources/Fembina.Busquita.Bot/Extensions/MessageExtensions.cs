using Talkie.Pipelines.Intercepting;
using Talkie.Signals;

namespace Fembina.Busquita.Bot.Extensions;

public static class MessageExtensions
{
    public static ISignalInterceptingPipelineBuilder<MessagePublishedSignal> OnlyCommand
    (
        this ISignalInterceptingPipelineBuilder<MessagePublishedSignal> builder,
        string commandName
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(commandName);

        var commandLength = commandName.Length;

        return builder.Where(signal =>
        {
            var content = signal.Message.Content;

            if (content.IsEmpty) return false;

            var textSpan = content.Text.AsSpan();

            if (textSpan[0] is not '/') return false;

            if (textSpan.Length < commandLength + 1) return false;

            var commandSpan = textSpan.Slice(1, commandLength);

            return commandSpan.Equals(commandName.AsSpan(), StringComparison.InvariantCultureIgnoreCase);
        });
    }
}
