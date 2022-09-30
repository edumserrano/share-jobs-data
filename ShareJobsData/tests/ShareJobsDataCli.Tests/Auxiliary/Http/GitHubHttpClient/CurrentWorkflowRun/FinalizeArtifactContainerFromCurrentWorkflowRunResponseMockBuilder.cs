namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.CurrentWorkflowRun;

internal sealed class FinalizeArtifactContainerFromCurrentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder<FinalizeArtifactContainerFromCurrentWorkflowRunResponseMockBuilder>
{
    private string? _runtimeUrl;
    private string? _runId;
    private string? _containerName;

    public FinalizeArtifactContainerFromCurrentWorkflowRunResponseMockBuilder FromCurrentWorkflowRun(string runtimeUrl, string runId, string containerName)
    {
        _runtimeUrl = runtimeUrl;
        _runId = runId;
        _containerName = containerName;
        return this;
    }

    protected override string OperationName { get; } = "finalize artifact container from current workflow run";

    protected override string GetRequestUrl() => $"{_runtimeUrl}_apis/pipelines/workflows/{_runId}/artifacts?api-version=6.0-preview&artifactName={_containerName}";
}
