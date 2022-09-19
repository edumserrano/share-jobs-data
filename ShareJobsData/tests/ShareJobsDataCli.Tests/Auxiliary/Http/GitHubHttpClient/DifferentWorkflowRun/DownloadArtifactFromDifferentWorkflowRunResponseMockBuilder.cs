namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.DifferentWorkflowRun;

internal class DownloadArtifactFromDifferentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder<DownloadArtifactFromDifferentWorkflowRunResponseMockBuilder>
{
    private string? _repoName;
    private string? _artifactId;

    public DownloadArtifactFromDifferentWorkflowRunResponseMockBuilder FromWorkflowArtifactId(string repoName, string artifactId)
    {
        _repoName = repoName;
        _artifactId = artifactId;
        return this;
    }

    public override void Build(HttpResponseMessageMockBuilder httpResponseMessageMockBuilder)
    {
        if (string.IsNullOrEmpty(_repoName) || string.IsNullOrEmpty(_artifactId))
        {
            throw new InvalidOperationException("Invalid response mock configuration for list artifacts from different workflow run");
        }

        httpResponseMessageMockBuilder
            .WhereRequestPathEquals($"/repos/{_repoName}/actions/artifacts/{_artifactId}/zip")
            .RespondWith(httpRequestMessage =>
            {
                var httpResponseMessage = new HttpResponseMessage(ResponseHttpStatusCode)
                {
                    // this is required when testing failure status codes because the app returns information from the http request made
                    RequestMessage = httpRequestMessage,
                };
                if (ResponseContentFilepath is not null)
                {
                    httpResponseMessage.Content = ResponseContentFilepath.ReadFileAsAzipContent();
                }

                return httpResponseMessage;
            });
    }
}
