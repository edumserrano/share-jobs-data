namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.Types;

internal sealed record GitHubArtifactContainerName
{
    private readonly string _value;

    public GitHubArtifactContainerName(string name)
    {
        _value = name.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubArtifactContainerName name)
    {
        return name._value;
    }

    public override string ToString() => (string)this;
}
