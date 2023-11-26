namespace ShareJobsDataCli.Common.Cli.Errors;

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
    private static CommandException CreateCommandException(string errorMessage) => new CommandException(errorMessage);
}
