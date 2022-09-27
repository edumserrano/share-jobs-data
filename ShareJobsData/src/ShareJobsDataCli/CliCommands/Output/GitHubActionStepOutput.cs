namespace ShareJobsDataCli.CliCommands.Output;

internal sealed class GitHubActionStepOutput
{
    private readonly IConsole _console;

    public GitHubActionStepOutput(IConsole console)
    {
        _console = console.NotNull();
    }

    public async Task WriteAsync(JobDataAsKeysAndValues jobDataKeysAndValues)
    {
        foreach (var (key, value) in jobDataKeysAndValues.KeysAndValues)
        {
            // need to sanitize value before setting it as a step output.
            // See:
            // - https://github.com/orgs/community/discussions/26288#discussioncomment-3251220
            // - https://trstringer.com/github-actions-multiline-strings/
            var sanitizedValue = value
                .Replace("%", "%25", StringComparison.InvariantCulture)
                .Replace("\n", "%0A", StringComparison.InvariantCulture)
                .Replace("\r", "%0D", StringComparison.InvariantCulture);
            await _console.Output.WriteLineAsync($"::set-output name={key}::{sanitizedValue}");
        }
    }
}
