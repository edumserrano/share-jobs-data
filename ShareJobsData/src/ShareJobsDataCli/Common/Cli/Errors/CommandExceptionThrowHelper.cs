namespace ShareJobsDataCli.Common.Cli.Errors;

// See /docs/dev-notes/code-details/throw-helper.md to understand more about this implementation.
internal static class CommandExceptionThrowHelper
{
    [DoesNotReturn]
    internal static void Throw(string command, string error)
    {
        command.NotNullOrWhiteSpace();
        error.NotNullOrWhiteSpace();

        var errorMessage = new ErrorMessageBuilder()
            .UseCommand(command)
            .UseError(error)
            .Build();
        throw CreateCommandException(errorMessage);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Exception CreateCommandException(string errorMessage) => new CommandException(errorMessage);
}
