namespace ShareJobsDataCli.CliCommands.Output;

internal sealed class GitHubActionStrictJsonStepOutput
{
    private readonly IConsole _console;

    public GitHubActionStrictJsonStepOutput(IConsole console)
    {
        _console = console.NotNull();
    }

    public async Task WriteToConsoleAsync(string json)
    {
        json.NotNull();
        var sanitizedOutput = json.SanitizeGitHubStepOutput();
        await _console.Output.WriteLineAsync($"::set-output name=data::{sanitizedOutput}");
    }
}
