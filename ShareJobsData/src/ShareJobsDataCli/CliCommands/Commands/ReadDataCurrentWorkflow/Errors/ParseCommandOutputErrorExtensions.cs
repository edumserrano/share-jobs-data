using static ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.Outputs.ParseReadDataFromCurrentGitHubWorkflowCommandOutputResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.Errors;

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
