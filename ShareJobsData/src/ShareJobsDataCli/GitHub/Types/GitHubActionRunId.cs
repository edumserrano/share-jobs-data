namespace ShareJobsDataCli.GitHub.Types;

internal sealed class GitHubActionRunId
{
    private readonly long _value;

    public GitHubActionRunId(string runId)
    {
        _value = runId.Positive();
    }

    public static implicit operator long(GitHubActionRunId runId)
    {
        return runId._value;
    }

    public override string ToString() => $"{_value}";
}
