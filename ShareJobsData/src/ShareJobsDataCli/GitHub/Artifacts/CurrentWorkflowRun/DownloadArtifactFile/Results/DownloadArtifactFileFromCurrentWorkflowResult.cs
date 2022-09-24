namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadArtifactFileFromCurrentWorkflowResult
{
    private DownloadArtifactFileFromCurrentWorkflowResult()
    {
    }

    public record Ok(GitHubArtifactItemContent GitHubArtifactItem)
        : DownloadArtifactFileFromCurrentWorkflowResult;

    public record Error()
        : DownloadArtifactFileFromCurrentWorkflowResult;

    public record ArtifactNotFound(GitHubArtifactContainerName ArtifactContainerName)
        : Error;

    public record ArtifactContainerItemNotFound(GitHubArtifactItemFilePath ArtifactItemFilePath)
        : Error;

    public record FailedToListWorkflowRunArtifacts(JsonHttpResult<GitHubArtifactContainers>.Error JsonHttpError)
        : Error;

    public record FailedToGetContainerItems(JsonHttpResult<GitHubArtifactContainerItems>.Error JsonHttpError)
        : Error;

    public record FailedToDownloadArtifact(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : Error;

    public static implicit operator DownloadArtifactFileFromCurrentWorkflowResult(GitHubArtifactItemContent gitHubArtifactItemContent) => new Ok(gitHubArtifactItemContent);

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
