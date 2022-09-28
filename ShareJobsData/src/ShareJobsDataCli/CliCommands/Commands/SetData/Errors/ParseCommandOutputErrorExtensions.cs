using static ShareJobsDataCli.CliCommands.Commands.SetData.Outputs.ParseSetDataCommandOutputResult;

namespace ShareJobsDataCli.CliCommands.Commands.SetData.Errors;

internal static class ParseCommandOutputErrorExtensions
{
    public static Task WriteToConsoleAsync(this UnknownOutput unkownOutput, IConsole console, string command)
    {
        unkownOutput.NotNull();
        console.NotNull();
        command.NotNullOrWhiteSpace();

        var error = "";
        return console.WriteErrorAsync(command, error);
    }
}
