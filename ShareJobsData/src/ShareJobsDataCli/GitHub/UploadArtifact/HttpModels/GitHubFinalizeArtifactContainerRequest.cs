namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

internal sealed record GitHubFinalizeArtifactContainerRequest
{
    public GitHubFinalizeArtifactContainerRequest(int artifactContainerSize)
    {
        Size = artifactContainerSize.Positive();
    }

    public int Size { get; }
}
