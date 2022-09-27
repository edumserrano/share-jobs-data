namespace ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact.HttpModels;

internal sealed record GitHubFinalizeArtifactContainerRequest
{
    public GitHubFinalizeArtifactContainerRequest(long artifactContainerSize)
    {
        Size = artifactContainerSize.Positive();
    }

    public long Size { get; }
}
