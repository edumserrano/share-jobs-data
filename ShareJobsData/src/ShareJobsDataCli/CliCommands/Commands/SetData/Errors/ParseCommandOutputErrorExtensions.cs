using static ShareJobsDataCli.CliCommands.Commands.SetData.Outputs.ParseSetDataCommandOutputResult;

namespace ShareJobsDataCli.CliCommands.Commands.SetData.Errors;

internal static class ParseCommandOutputErrorExtensions
{
    public static Task WriteToConsoleAsync(this UnknownOutput unknownOutput, IConsole console, string command)
    {
        unknownOutput.NotNull();
        console.NotNull();
        command.NotNullOrWhiteSpace();

        var error = $"Option --output has been provided with an invalid value: '{unknownOutput.OutputOptionValue}'. It must be one of: strict-json, github-step-json.";
        return console.WriteErrorAsync(command, error);
    }
}
