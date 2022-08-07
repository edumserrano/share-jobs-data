namespace ShareJobsDataCli.GitHub;

public interface IGitHubEnvironment
{
    string ActionRuntimeToken { get; }

    string ActionRuntimeUrl { get; }

    string ActionRunId { get; }
}

internal class GitHubEnvironment : IGitHubEnvironment
{
    public GitHubEnvironment()
    {
        ActionRuntimeToken = Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_TOKEN") ?? string.Empty;
        ActionRuntimeUrl = Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_URL") ?? string.Empty;
        ActionRunId = Environment.GetEnvironmentVariable("GITHUB_RUN_ID") ?? string.Empty;
    }

    public string ActionRuntimeToken { get; }

    public string ActionRuntimeUrl { get; }

    public string ActionRunId { get; }
}
