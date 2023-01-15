namespace ShareJobsDataCli.Common.GitHub;

public interface IGitHubEnvironment
{
    string GitHubActionRuntimeToken { get; }

    string GitHubActionRuntimeUrl { get; }

    string GitHubActionRunId { get; }

    string GitHubRepository { get; }

    string GitHubOutputFile { get; }
}

internal sealed record GitHubEnvironment : IGitHubEnvironment
{
    public string GitHubActionRuntimeToken { get; } = Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_TOKEN") ?? string.Empty;

    public string GitHubActionRuntimeUrl { get; } = Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_URL") ?? string.Empty;

    public string GitHubActionRunId { get; } = Environment.GetEnvironmentVariable("GITHUB_RUN_ID") ?? string.Empty;

    public string GitHubRepository { get; } = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY") ?? string.Empty;

    public string GitHubOutputFile { get; } = Environment.GetEnvironmentVariable("GITHUB_OUTPUT") ?? string.Empty;
}
