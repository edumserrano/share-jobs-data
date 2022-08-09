namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.HttpModels.Requests;

internal sealed record GitHubFinalizeArtifactContainerRequest
{
    public GitHubFinalizeArtifactContainerRequest(int artifactContainerSize)
    {
        Size = artifactContainerSize.Positive();
    }

    public int Size { get; }
}
