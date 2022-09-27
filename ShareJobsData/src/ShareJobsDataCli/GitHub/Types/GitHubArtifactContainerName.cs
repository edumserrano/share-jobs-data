namespace ShareJobsDataCli.GitHub.Types;

internal sealed record GitHubArtifactContainerName
{
    private readonly string _value;

    public GitHubArtifactContainerName(string artifactName)
    {
        _value = artifactName.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubArtifactContainerName artifactName)
    {
        return artifactName._value;
    }

    public override string ToString() => (string)this;
}
