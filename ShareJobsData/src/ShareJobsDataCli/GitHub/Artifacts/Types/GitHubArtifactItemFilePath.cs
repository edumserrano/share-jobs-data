namespace ShareJobsDataCli.GitHub.Artifacts.Types;

internal sealed record GitHubArtifactItemFilePath
{
    private readonly string _value;

    public GitHubArtifactItemFilePath(
        GitHubArtifactContainerName containerName,
        string path)
    {
        _value = $"{containerName.NotNull()}/{path.NotNullOrWhiteSpace()}";
    }

    public static implicit operator string(GitHubArtifactItemFilePath filePath)
    {
        return filePath._value;
    }

    public override string ToString() => (string)this;
}
