namespace ShareJobsDataCli.GitHub;

public interface IGitHubEnvironment
{
    string GitHubActionRuntimeToken { get; }

    string GitHubActionRuntimeUrl { get; }

    string GitHubActionRunId { get; }

    string GitHubRepository { get; }
}

internal record GitHubEnvironment : IGitHubEnvironment
{
    public GitHubEnvironment()
    {
        GitHubActionRuntimeToken = Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_TOKEN") ?? string.Empty;
        GitHubActionRuntimeUrl = Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_URL") ?? string.Empty;
        GitHubActionRunId = Environment.GetEnvironmentVariable("GITHUB_RUN_ID") ?? string.Empty;
        GitHubRepository = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY") ?? string.Empty;
    }

    public string GitHubActionRuntimeToken { get; }

    public string GitHubActionRuntimeUrl { get; }

    public string GitHubActionRunId { get; }

    public string GitHubRepository { get; }
}