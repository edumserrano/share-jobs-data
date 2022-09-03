namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadArtifactFileFromDifferentWorkflowResult
{
    private DownloadArtifactFileFromDifferentWorkflowResult()
    {
    }

    public record Ok(GitHubArtifactItemContent GitHubArtifactItem)
        : DownloadArtifactFileFromDifferentWorkflowResult;

    public record Error()
        : DownloadArtifactFileFromDifferentWorkflowResult;

    public record ArtifactNotFound(GitHubRepositoryName RepoName, GitHubRunId WorkflowRunId, GitHubArtifactContainerName ArtifactContainerName)
        : Error;

    public record ArtifactFileNotFound(GitHubRepositoryName RepoName, GitHubRunId WorkflowRunId, GitHubArtifactContainerName ArtifactContainerName, GitHubArtifactItemFilename ArtifactItemFilename)
        : Error;

    public record FailedToListWorkflowRunArtifacts(JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>.Error ErrorResult)
        : Error;

    public record FailedToDownloadArtifact(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : Error;

    public static implicit operator DownloadArtifactFileFromDifferentWorkflowResult(GitHubArtifactItemContent gitHubArtifactItemContent) => new Ok(gitHubArtifactItemContent);

    public bool IsOk(
       [NotNullWhen(returnValue: true)] out GitHubArtifactItemContent? gitHubArtifactItem,
       [NotNullWhen(returnValue: false)] out Error? error)
    {
        gitHubArtifactItem = null;
        error = null;

        if (this is Ok ok)
        {
            gitHubArtifactItem = ok.GitHubArtifactItem;
            return true;
        }

        error = (Error)this;
        return false;
    }
}
