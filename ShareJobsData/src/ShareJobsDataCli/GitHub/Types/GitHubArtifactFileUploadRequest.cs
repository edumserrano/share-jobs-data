namespace ShareJobsDataCli.GitHub.Types;

internal sealed record GitHubArtifactFileUploadRequest
{
    public GitHubArtifactFileUploadRequest(GitHubArtifactItemFilePath filePath, string filePayload)
    {
        FilePath = filePath.NotNull();
        FilePayload = filePayload.NotNullOrWhiteSpace();
    }

    public GitHubArtifactItemFilePath FilePath { get; }

    public string FilePayload { get; }
}
