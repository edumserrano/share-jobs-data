namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient.DifferentWorkflowRun;

internal sealed class DownloadArtifactFromDifferentWorkflowRunResponseMockBuilder : BaseResponseMockBuilder<DownloadArtifactFromDifferentWorkflowRunResponseMockBuilder>
{
    private string? _repoName;
    private string? _artifactId;

    public DownloadArtifactFromDifferentWorkflowRunResponseMockBuilder FromWorkflowArtifactId(string repoName, string artifactId)
    {
        _repoName = repoName;
        _artifactId = artifactId;
        return this;
    }

    protected override string OperationName { get; } = "download artifact from different workflow run";

    protected override string? GetRequestUrl() => $"https://api.github.com/repos/{_repoName}/actions/artifacts/{_artifactId}/zip";

    protected override HttpContent? GetResponseContent()
    {
        return ResponseContentFilepath?.ReadFileAsZipContent();
    }
}
