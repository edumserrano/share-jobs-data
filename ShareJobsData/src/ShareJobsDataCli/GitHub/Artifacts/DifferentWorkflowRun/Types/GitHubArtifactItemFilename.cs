namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.Types;

internal sealed record GitHubArtifactItemFilename
{
    private readonly string _value;

    public GitHubArtifactItemFilename(string filename)
    {
        _value = filename.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubArtifactItemFilename filename)
    {
        return filename._value;
    }

    public override string ToString() => (string)this;
}
