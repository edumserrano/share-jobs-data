namespace ShareJobsDataCli.Common.Cli.Output;

internal sealed class GitHubActionStepJsonStepOutput
{
    private readonly IConsole _console;

    public GitHubActionStepJsonStepOutput(IConsole console)
    {
        _console = console.NotNull();
    }

    public async Task WriteToConsoleAsync(JobDataAsKeysAndValues jobDataKeysAndValues)
    {
        jobDataKeysAndValues.NotNull();

        foreach (var (key, value) in jobDataKeysAndValues.KeysAndValues)
        {
            // the GitHub step output doesn't support a notation with several dots in the key of the step output.
            // For instance 'addresses.home.street' is invalid.
            //
            // To deal with this we replace certain JSON syntax characters by underscore so that the value becomes
            // valid for a step output key.
            // So as an example:
            // 'addresses.home.street' becomes 'addresses_home_street'
            // 'addresses[0].street' becomes 'addresses_0_street'
            var sanitizedKey = key
                .Replace(".", "_", StringComparison.InvariantCulture)
                .Replace("[", "_", StringComparison.InvariantCulture)
                .Replace("]", "_", StringComparison.InvariantCulture);
            await _console.Output.WriteGitHubStepOuputAsync(sanitizedKey, value);
        }
    }
}
