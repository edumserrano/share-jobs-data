namespace ShareJobsDataCli.GitHub.Types;

internal sealed record GitHubArtifactItemContent
{
    private readonly string _value;

    public GitHubArtifactItemContent(string content)
    {
        _value = content.NotNull();
    }

    public static implicit operator string(GitHubArtifactItemContent content)
    {
        return content._value;
    }

    public override string ToString() => (string)this;
}
