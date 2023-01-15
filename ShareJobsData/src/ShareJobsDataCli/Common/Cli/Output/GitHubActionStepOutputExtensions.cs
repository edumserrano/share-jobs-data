namespace ShareJobsDataCli.Common.Cli.Output;

internal static class GitHubActionStepOutputExtensions
{
    // Before you had to sanitize value before setting them as a step output.
    // See:
    // - https://github.com/orgs/community/discussions/26288#discussioncomment-3251220
    // - https://trstringer.com/github-actions-multiline-strings/
    //
    // As of this change https://github.blog/changelog/2022-10-11-github-actions-deprecating-save-state-and-set-output-commands/
    // You do not need to sanitize values anymore, for multi line values you can use a delimiter
    // which is what this function does. See: https://docs.github.com/en/actions/using-workflows/workflow-commands-for-github-actions#multiline-strings
    // For more info on this see discussion here: https://github.com/orgs/community/discussions/26288#discussioncomment-3876281
    public static async Task WriteGitHubStepOuputAsync(
        this ConsoleWriter consoleWriter,
        string key,
        string value)
    {
        // only need to use a delimiter for multi line values but for simplicity we always use one
        var delimiter = $"EOF_{BitConverter.ToString(RandomNumberGenerator.GetBytes(8))}";
        await consoleWriter.WriteLineAsync($"{key}<<{delimiter}");
        await consoleWriter.WriteLineAsync(value);
        await consoleWriter.WriteLineAsync(delimiter);
    }
}
