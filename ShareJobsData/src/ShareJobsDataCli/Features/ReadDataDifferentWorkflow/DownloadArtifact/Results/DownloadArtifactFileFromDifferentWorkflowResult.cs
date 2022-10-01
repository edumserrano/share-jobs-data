namespace ShareJobsDataCli.Features.ReadDataDifferentWorkflow.DownloadArtifact.Results;

internal abstract record DownloadArtifactFileFromDifferentWorkflowResult
{
    private DownloadArtifactFileFromDifferentWorkflowResult()
    {
    }

    public sealed record Ok(GitHubArtifactItemJsonContent GitHubArtifactItem)
        : DownloadArtifactFileFromDifferentWorkflowResult;

    public abstract record Error
        : DownloadArtifactFileFromDifferentWorkflowResult;

    public sealed record ArtifactNotFound(GitHubRepositoryName RepoName, GitHubRunId WorkflowRunId, GitHubArtifactContainerName ArtifactContainerName)
        : Error;

    public sealed record ArtifactFileNotFound(GitHubRepositoryName RepoName, GitHubRunId WorkflowRunId, GitHubArtifactContainerName ArtifactContainerName, GitHubArtifactItemFilename ArtifactItemFilename)
        : Error;

    public sealed record FailedToListWorkflowRunArtifacts(JsonHttpResult<GitHubListWorkflowRunArtifactsHttpResponse>.Error JsonHttpError)
        : Error;

    public sealed record FailedToDownloadArtifact(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : Error;

    public sealed record ArtifactItemContentNotJson(GitHubArtifactItemNotJsonContent NotJsonContent)
        : Error;

    public static implicit operator DownloadArtifactFileFromDifferentWorkflowResult(GitHubArtifactItemJsonContent gitHubArtifactItemContent) => new Ok(gitHubArtifactItemContent);

    public bool IsOk(
       [NotNullWhen(returnValue: true)] out GitHubArtifactItemJsonContent? gitHubArtifactItem,
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
