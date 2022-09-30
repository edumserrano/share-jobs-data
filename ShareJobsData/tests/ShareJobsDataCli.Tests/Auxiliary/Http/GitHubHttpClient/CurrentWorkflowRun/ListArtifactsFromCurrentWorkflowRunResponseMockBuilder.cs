namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.CurrentWorkflowRun;

internal sealed class ListArtifactsFromCurrentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder<ListArtifactsFromCurrentWorkflowRunResponseMockBuilder>
{
    private string? _runtimeUrl;
    private string? _runId;

    public ListArtifactsFromCurrentWorkflowRunResponseMockBuilder FromCurrentWorkflowRun(string runtimeUrl, string runId)
    {
        _runtimeUrl = runtimeUrl;
        _runId = runId;
        return this;
    }

    protected override string OperationName { get; } = "list artifacts from current workflow run";

    protected override string GetRequestUrl() => $"{_runtimeUrl}_apis/pipelines/workflows/{_runId}/artifacts?api-version=6.0-preview";
}
