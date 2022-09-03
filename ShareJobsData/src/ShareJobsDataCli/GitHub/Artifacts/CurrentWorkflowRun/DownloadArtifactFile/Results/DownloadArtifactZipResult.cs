namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadContainerItemResult
{
    private DownloadContainerItemResult()
    {
    }

    public record Ok(GitHubArtifactItemContent ArtifactItemContent)
        : DownloadContainerItemResult;

    public record FailedToDownloadContainerItem(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : DownloadContainerItemResult;

    public static implicit operator DownloadContainerItemResult(GitHubArtifactItemContent artifactItemContent) => new Ok(artifactItemContent);

    public bool IsOk(
        [NotNullWhen(returnValue: true)] out GitHubArtifactItemContent? artifactItemContent,
        [NotNullWhen(returnValue: false)] out FailedStatusCodeHttpResponse? failedStatusCodeHttpResponse)
    {
        artifactItemContent = null;
        failedStatusCodeHttpResponse = null;

        if (this is Ok ok)
        {
            artifactItemContent = ok.ArtifactItemContent;
            return true;
        }

        var failedToDownloadArtifact = (FailedToDownloadContainerItem)this;
        failedStatusCodeHttpResponse = failedToDownloadArtifact.FailedStatusCodeHttpResponse;
        return false;
    }
}
