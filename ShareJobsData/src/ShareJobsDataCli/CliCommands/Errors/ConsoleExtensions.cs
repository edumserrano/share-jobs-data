namespace ShareJobsDataCli.CliCommands.Errors;

internal static class ConsoleExtensions
{
    public static Task WriteErrorAsync(this IConsole console, string command, string error)
    {
        console.NotNull();
        var errorMessage = new ConsoleErrorMessageBuilder()
            .UseCommand(command)
            .UseError(error)
            .Build();
        return console.WriteErrorAsync(errorMessage);
    }

    public static async Task WriteErrorAsync(this IConsole console, string error)
    {
        console.NotNull();
        error.NotNullOrWhiteSpace();
        using var _ = console.WithForegroundColor(ConsoleColor.Red);
        await console.Error.WriteAsync(error);
    }
}
