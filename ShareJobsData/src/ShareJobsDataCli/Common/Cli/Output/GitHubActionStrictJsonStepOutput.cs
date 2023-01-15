namespace ShareJobsDataCli.Common.Cli.Output;

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
        await _console.Output.WriteGitHubStepOuputAsync(key: "data", json);
    }
}
