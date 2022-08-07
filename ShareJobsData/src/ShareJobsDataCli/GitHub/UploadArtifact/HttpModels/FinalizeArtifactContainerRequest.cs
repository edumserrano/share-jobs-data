namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

internal record FinalizeArtifactContainerRequest
{
    public FinalizeArtifactContainerRequest(int artifactContainerSize)
    {
        Size = artifactContainerSize.Positive();
    }

    public int Size { get; }
}
