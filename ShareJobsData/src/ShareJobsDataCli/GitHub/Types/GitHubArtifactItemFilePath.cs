namespace ShareJobsDataCli.GitHub.Types;

internal sealed record GitHubArtifactItemFilePath
{
    private readonly string _value;

    public GitHubArtifactItemFilePath(
        GitHubArtifactContainerName containerName,
        string artifactFilename)
    {
        _value = $"{containerName.NotNull()}/{artifactFilename.NotNullOrWhiteSpace()}";
    }

    public static implicit operator string(GitHubArtifactItemFilePath filePath)
    {
        return filePath._value;
    }

    public override string ToString() => this;
}
