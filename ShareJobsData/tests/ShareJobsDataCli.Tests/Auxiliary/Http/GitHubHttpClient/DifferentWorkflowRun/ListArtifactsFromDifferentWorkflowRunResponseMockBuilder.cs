namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.DifferentWorkflowRun;

internal class ListArtifactsFromDifferentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder<ListArtifactsFromDifferentWorkflowRunResponseMockBuilder>
{
    private string? _repoName;
    private string? _runId;

    public ListArtifactsFromDifferentWorkflowRunResponseMockBuilder FromWorkflowRun(string repoName, string runId)
    {
        _repoName = repoName;
        _runId = runId;
        return this;
    }

    protected override string OperationName { get; } = "list artifacts from different workflow run";

    protected override string? GetRequestUrl() => $"https://api.github.com/repos/{_repoName}/actions/runs/{_runId}/artifacts";
}
