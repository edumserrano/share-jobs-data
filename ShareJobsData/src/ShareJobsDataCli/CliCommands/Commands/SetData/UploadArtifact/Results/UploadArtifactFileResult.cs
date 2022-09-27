namespace ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact.Results;

internal abstract record UploadArtifactFileResult
{
    private UploadArtifactFileResult()
    {
    }

    public record Ok(GitHubFinalizeArtifactContainerHttpResponse GitHubArtifactContainer)
        : UploadArtifactFileResult;

    public abstract record Error()
        : UploadArtifactFileResult;

    public record FailedToCreateArtifactContainer(JsonHttpResult<GitHubCreateArtifactContainerHttpResponse>.Error JsonHttpError)
        : Error;

    public record FailedToUploadArtifact(JsonHttpResult<GitHubUploadArtifactFileHttpResponse>.Error JsonHttpError)
        : Error;

    public record FailedToFinalizeArtifactContainer(JsonHttpResult<GitHubFinalizeArtifactContainerHttpResponse>.Error JsonHttpError)
        : Error;

    public static implicit operator UploadArtifactFileResult(GitHubFinalizeArtifactContainerHttpResponse gitHubArtifactContainer) => new Ok(gitHubArtifactContainer);

    public bool IsOk(
       [NotNullWhen(returnValue: true)] out GitHubFinalizeArtifactContainerHttpResponse? gitHubArtifactContainer,
       [NotNullWhen(returnValue: false)] out Error? error)
    {
        gitHubArtifactContainer = null;
        error = null;

        if (this is Ok ok)
        {
            gitHubArtifactContainer = ok.GitHubArtifactContainer;
            return true;
        }

        error = (Error)this;
        return false;
    }
}
