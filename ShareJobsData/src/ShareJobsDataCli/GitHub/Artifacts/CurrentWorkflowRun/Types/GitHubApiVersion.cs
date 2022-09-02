namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.Types;

internal sealed record GitHubApiVersion
{
    private readonly string _value;

    public static GitHubApiVersion Latest { get; } = new GitHubApiVersion("6.0-preview");

    public GitHubApiVersion(string version)
    {
        _value = version.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubApiVersion version)
    {
        return version._value;
    }

    public override string ToString() => (string)this;
}