namespace ShareJobsDataCli.GitHub.Artifacts.Types;

internal sealed record GitHubAuthToken
{
    private readonly string _value;

    public GitHubAuthToken(string gitHubAuthToken)
    {
        _value = gitHubAuthToken.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubAuthToken gitHubAuthToken)
    {
        return gitHubAuthToken._value;
    }

    public override string ToString() => (string)this;
}