namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.DifferentWorkflowRun;

internal class ListArtifactsFromDifferentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder<ListArtifactsFromDifferentWorkflowRunResponseMockBuilder>
{
    private string? _repoName;
    private string? _runId;
    private string? _responseContentAsString;

    public ListArtifactsFromDifferentWorkflowRunResponseMockBuilder FromWorkflowRun(string repoName, string runId)
    {
        _repoName = repoName;
        _runId = runId;
        return this;
    }

    public ListArtifactsFromDifferentWorkflowRunResponseMockBuilder WithResponseContent(string responseContent)
    {
        _responseContentAsString = responseContent;
        return this;
    }

    public override void Build(HttpResponseMessageMockBuilder httpResponseMessageMockBuilder)
    {
        if (string.IsNullOrEmpty(_repoName) || string.IsNullOrEmpty(_runId))
        {
            throw new InvalidOperationException("Invalid response mock configuration for list artifacts from different workflow run");
        }

        httpResponseMessageMockBuilder
            .WhereRequestPathEquals($"/repos/{_repoName}/actions/runs/{_runId}/artifacts")
            .RespondWith(httpRequestMessage =>
            {
                var httpResponseMessage = new HttpResponseMessage(ResponseHttpStatusCode)
                {
                    // this is required when testing failure status codes because the app returns information from the http request made
                    RequestMessage = httpRequestMessage,
                };
                if (ResponseContentFilepath is not null)
                {
                    httpResponseMessage.Content = ResponseContentFilepath.ReadFileAsStringContent();
                }

                if (_responseContentAsString is not null)
                {
                    httpResponseMessage.Content = new StringContent(_responseContentAsString);
                }

                return httpResponseMessage;
            });
    }
}
