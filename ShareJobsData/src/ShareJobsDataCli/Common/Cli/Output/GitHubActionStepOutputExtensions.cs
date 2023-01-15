namespace ShareJobsDataCli.Common.Cli.Output;

internal static class GitHubActionStepOutputExtensions
{
    // need to sanitize value before setting it as a step output.
    // See:
    // - https://github.com/orgs/community/discussions/26288#discussioncomment-3251220
    // - https://trstringer.com/github-actions-multiline-strings/
    public static string SanitizeGitHubStepOutput(this string value)
    {
        return value
            .Replace("%", "%25", StringComparison.InvariantCulture)
            .Replace("\n", "%0A", StringComparison.InvariantCulture)
            .Replace("\r", "%0D", StringComparison.InvariantCulture);
    }
}
