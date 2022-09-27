namespace ShareJobsDataCli.GitHub.Types;

internal sealed record GitHubArtifactFileUploadRequest
{
    public GitHubArtifactFileUploadRequest(GitHubArtifactItemFilePath filePath, string fileUploadContent)
    {
        FilePath = filePath.NotNull();
        FileUploadContent = fileUploadContent.NotNullOrWhiteSpace();
    }

    public GitHubArtifactItemFilePath FilePath { get; }

    public string FileUploadContent { get; }
}
