namespace ShareJobsDataCli.GitHub.UploadArtifact.Types;

internal record GitHubUploadArtifact
{
    public GitHubUploadArtifact(
        string containerName,
        string filePath,
        string filePayload)
    {
        ContainerName = containerName.NotNullOrWhiteSpace();
        FilePath = $"{ContainerName}/{filePath.NotNullOrWhiteSpace()}";
        FilePayload = filePayload.NotNullOrWhiteSpace();
    }

    public string ContainerName { get; }

    public string FilePath { get; }

    public string FilePayload { get; }
}
