namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact.Results;

internal abstract record DownloadArtifactFileFromCurrentWorkflowResult
{
    private DownloadArtifactFileFromCurrentWorkflowResult()
    {
    }

    public sealed record Ok(GitHubArtifactItemJsonContent GitHubArtifactItem)
        : DownloadArtifactFileFromCurrentWorkflowResult;

    public abstract record Error
        : DownloadArtifactFileFromCurrentWorkflowResult;

    public sealed record ArtifactNotFound(GitHubArtifactContainerName ArtifactContainerName)
        : Error;

    public sealed record ArtifactContainerItemNotFound(GitHubArtifactItemFilePath ArtifactItemFilePath)
        : Error;

    public sealed record FailedToListWorkflowRunArtifacts(JsonHttpResult<GitHubListArtifactsHttpResponse>.Error JsonHttpError)
        : Error;

    public sealed record FailedToGetContainerItems(JsonHttpResult<GitHubGetContainerItemsHttpResponse>.Error JsonHttpError)
        : Error;

    public sealed record FailedToDownloadArtifact(DownloadContainerItemResult.Error DownloadContainerItemError)
        : Error;

    public static implicit operator DownloadArtifactFileFromCurrentWorkflowResult(GitHubArtifactItemJsonContent gitHubArtifactItemContent) => new Ok(gitHubArtifactItemContent);

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
