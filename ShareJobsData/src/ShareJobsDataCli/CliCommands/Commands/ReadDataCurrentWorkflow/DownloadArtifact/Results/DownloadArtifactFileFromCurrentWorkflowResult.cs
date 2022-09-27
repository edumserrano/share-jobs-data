namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact.Results;

internal abstract record DownloadArtifactFileFromCurrentWorkflowResult
{
    private DownloadArtifactFileFromCurrentWorkflowResult()
    {
    }

    public record Ok(GitHubArtifactItemJsonContent GitHubArtifactItem)
        : DownloadArtifactFileFromCurrentWorkflowResult;

    public abstract record Error()
        : DownloadArtifactFileFromCurrentWorkflowResult;

    public record ArtifactNotFound(GitHubArtifactContainerName ArtifactContainerName)
        : Error;

    public record ArtifactContainerItemNotFound(GitHubArtifactItemFilePath ArtifactItemFilePath)
        : Error;

    public record FailedToListWorkflowRunArtifacts(JsonHttpResult<GitHubListArtifactsHttpResponse>.Error JsonHttpError)
        : Error;

    public record FailedToGetContainerItems(JsonHttpResult<GitHubGetContainerItemsHttpResponse>.Error JsonHttpError)
        : Error;

    public record FailedToDownloadArtifact(DownloadContainerItemResult.Error DownloadContainerItemError)
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
