namespace ShareJobsDataCli.GitHub.Types;

internal sealed class GitHubActionRunId
{
    private readonly int _value;

    public GitHubActionRunId(string runId)
    {
        _value = runId.Positive();
    }

    public static implicit operator int(GitHubActionRunId runId)
    {
        return runId._value;
    }

    public override string ToString() => $"{_value}";
}
