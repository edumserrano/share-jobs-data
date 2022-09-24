namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadContainerItemResult
{
    private DownloadContainerItemResult()
    {
    }

    public record Ok(GitHubArtifactItemContent ArtifactItemContent)
        : DownloadContainerItemResult;

    public record Error()
        : DownloadContainerItemResult;

    public record ArtifactItemContentNotJson(string ArtifactItemContent)
        : Error;

    public record FailedToDownloadContainerItem(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : Error;

    public static implicit operator DownloadContainerItemResult(GitHubArtifactItemContent artifactItemContent) => new Ok(artifactItemContent);

    public bool IsOk(
        [NotNullWhen(returnValue: true)] out GitHubArtifactItemContent? artifactItemContent,
        [NotNullWhen(returnValue: false)] out Error? error)
    {
        artifactItemContent = null;
        error = null;

        if (this is Ok ok)
        {
            artifactItemContent = ok.ArtifactItemContent;
            return true;
        }

        error = (Error)this;
        return false;
    }
}
