namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.CurrentWorkflowRun;

internal sealed class DownloadArtifactFromCurrentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder<DownloadArtifactFromCurrentWorkflowRunResponseMockBuilder>
{
    private string? _containerItemLocation;

    public DownloadArtifactFromCurrentWorkflowRunResponseMockBuilder FromContainerItemLocation(string containerItemLocation)
    {
        _containerItemLocation = containerItemLocation;
        return this;
    }

    protected override string OperationName { get; } = "download artifact from current workflow run";

    protected override string? GetRequestUrl() => _containerItemLocation;
}
