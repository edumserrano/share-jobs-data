using ShareJobsDataCli.ArgumentValidations;

namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.HttpModels.Requests;

internal sealed record GitHubFinalizeArtifactContainerRequest
{
    public GitHubFinalizeArtifactContainerRequest(long artifactContainerSize)
    {
        Size = artifactContainerSize.Positive();
    }

    public long Size { get; }
}
