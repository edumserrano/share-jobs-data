namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadArtifactFileFromCurrentWorkflowResult
{
    private DownloadArtifactFileFromCurrentWorkflowResult()
    {
    }

    public record Ok(GitHubArtifactItemContent GitHubArtifactItem)
        : DownloadArtifactFileFromCurrentWorkflowResult;

    public record ArtifactNotFound(GitHubArtifactContainerName ArtifactContainerName)
        : DownloadArtifactFileFromCurrentWorkflowResult;

    public record ArtifactFileNotFound(GitHubArtifactItemFilePath ArtifactItemFilePath)
        : DownloadArtifactFileFromCurrentWorkflowResult;

    public static implicit operator DownloadArtifactFileFromCurrentWorkflowResult(GitHubArtifactItemContent gitHubArtifactItemContent) => new Ok(gitHubArtifactItemContent);
}
