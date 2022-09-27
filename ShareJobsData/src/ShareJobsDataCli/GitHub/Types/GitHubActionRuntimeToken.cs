namespace ShareJobsDataCli.GitHub.Types;

internal sealed record GitHubActionRuntimeToken
{
    private readonly string _value;

    public GitHubActionRuntimeToken(string runtimeToken)
    {
        _value = runtimeToken.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubActionRuntimeToken runtimeToken)
    {
        return runtimeToken._value;
    }

    public override string ToString() => (string)this;
}
