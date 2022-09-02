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

    public record FailedToListWorkflowRunArtifacts(JsonHttpResult<GitHubWorkflowRunArtifactsHttpResponse>.Error ErrorResult)
        : DownloadArtifactFileFromDifferentWorkflowResult;

    public record FailedToDownloadArtifact(DownloadArtifactZipResult.Error ErrorResult)
        : DownloadArtifactFileFromDifferentWorkflowResult;

    public static implicit operator DownloadArtifactFileFromDifferentWorkflowResult(GitHubArtifactItemContent gitHubArtifactItemContent) => new Ok(gitHubArtifactItemContent);
}
