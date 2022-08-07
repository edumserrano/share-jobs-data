namespace ShareJobsDataCli.GitHub.Types;

internal sealed class GitHubArtifactId
{
    private readonly int _value;

    public GitHubArtifactId(int artifactId)
    {
        _value = artifactId.Positive();
    }

    public static implicit operator int(GitHubArtifactId artifactId)
    {
        return artifactId._value;
    }

    public override string ToString() => $"{_value}";
}
