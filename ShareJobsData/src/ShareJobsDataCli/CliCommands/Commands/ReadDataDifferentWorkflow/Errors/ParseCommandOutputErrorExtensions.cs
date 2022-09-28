using static ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.Outputs.ParseReadDataFromDifferentGitHubWorkflowCommandOutputResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.Errors;

internal static class ParseCommandOutputErrorExtensions
{
    public static Task WriteToConsoleAsync(this UnknownOutput unkownOutput, IConsole console, string command)
    {
        unkownOutput.NotNull();
        console.NotNull();
        command.NotNullOrWhiteSpace();

        var error = $"Option --output has been provided with an invalid value: '{unkownOutput.OutputOptionValue}'. It must be one of: strict-json, github-step-json.";
        return console.WriteErrorAsync(command, error);
    }
}
