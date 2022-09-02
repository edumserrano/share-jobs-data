namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadArtifactFileFromDifferentWorkflowResult
{
    private DownloadArtifactFileFromDifferentWorkflowResult()
    {
    }

    public record Ok(GitHubArtifactItemContent GitHubArtifactItem)
        : DownloadArtifactFileFromDifferentWorkflowResult;

    public record ArtifactNotFound(GitHubRepositoryName RepoName, GitHubRunId WorkflowRunId, GitHubArtifactContainerName ArtifactContainerName)
        : DownloadArtifactFileFromDifferentWorkflowResult;

    public record ArtifactFileNotFound(GitHubRepositoryName RepoName, GitHubRunId WorkflowRunId, GitHubArtifactContainerName ArtifactContainerName, GitHubArtifactItemFilename ArtifactItemFilename)
        : DownloadArtifactFileFromDifferentWorkflowResult;

    public record FailedToListWorkflowRunArtifacts : DownloadArtifactFileFromDifferentWorkflowResult
    {
        public FailedToListWorkflowRunArtifacts(JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse> errorResult)
        {
            if (errorResult is JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>.Ok)
            {
                NotAnErrorResultException.Throw(errorResult);
            }

            ErrorResult = errorResult;
        }

        public JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse> ErrorResult { get; init; }
    }

    public record FailedToDownloadArtifact : DownloadArtifactFileFromDifferentWorkflowResult
    {
        public FailedToDownloadArtifact(DownloadArtifactZipResult errorResult)
        {
            if (errorResult is DownloadArtifactZipResult.Ok)
            {
                NotAnErrorResultException.Throw(errorResult);
            }

            ErrorResult = errorResult;
        }

        public DownloadArtifactZipResult ErrorResult { get; }
    }

    public static implicit operator DownloadArtifactFileFromDifferentWorkflowResult(GitHubArtifactItemContent gitHubArtifactItemContent) => new Ok(gitHubArtifactItemContent);
}
