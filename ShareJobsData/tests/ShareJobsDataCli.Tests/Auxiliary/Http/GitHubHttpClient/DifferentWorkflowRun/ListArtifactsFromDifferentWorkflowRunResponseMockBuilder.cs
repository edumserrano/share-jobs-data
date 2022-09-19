namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.DifferentWorkflowRun;

internal class ListArtifactsFromDifferentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder
{
    private string? _repoName;
    private string? _runId;

    public ListArtifactsFromDifferentWorkflowRunResponseMockBuilder FromWorkflowRun(string repoName, string runId)
    {
        _repoName = repoName;
        _runId = runId;
        return this;
    }

    public override void Build(HttpResponseMessageMockBuilder httpResponseMessageMockBuilder)
    {
        if (string.IsNullOrEmpty(_repoName)
            || string.IsNullOrEmpty(_runId)
            || ResponseContentFilepath is null)
        {
            throw new InvalidOperationException("Invalid response mock configuration for list artifacts from different workflow run");
        }

        httpResponseMessageMockBuilder
            .WhereRequestPathEquals($"/repos/{_repoName}/actions/runs/{_runId}/artifacts")
            .RespondWith(httpRequestMessage =>
            {
                return new HttpResponseMessage(ResponseHttpStatusCode)
                {
                    RequestMessage = httpRequestMessage, // this is required when testing failure status codes because the app returns information from the http request made
                    Content = ResponseContentFilepath.ReadFileAsStringContent(),
                };
            });
    }
}
