namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.UploadArtifactFile.Results;

internal abstract record UploadArtifactFileResult
{
    private UploadArtifactFileResult()
    {
    }

    public record Ok(GitHubArtifactContainer GitHubArtifactContainer)
        : UploadArtifactFileResult;

    public abstract record Error()
        : UploadArtifactFileResult;

    public record FailedToCreateArtifactContainer(JsonHttpResult<GitHubArtifactContainer>.Error JsonHttpError)
        : Error;

    public record FailedToUploadArtifact(JsonHttpResult<GitHubArtifactItem>.Error JsonHttpError)
        : Error;

    public record FailedToFinalizeArtifactContainer(JsonHttpResult<GitHubArtifactContainer>.Error JsonHttpError)
        : Error;

    public static implicit operator UploadArtifactFileResult(GitHubArtifactContainer gitHubArtifactContainer) => new Ok(gitHubArtifactContainer);

    public bool IsOk(
       [NotNullWhen(returnValue: true)] out GitHubArtifactContainer? gitHubArtifactContainer,
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