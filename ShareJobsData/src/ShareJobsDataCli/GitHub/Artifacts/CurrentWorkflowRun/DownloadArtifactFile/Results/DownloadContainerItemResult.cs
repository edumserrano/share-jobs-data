namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadContainerItemResult
{
    private DownloadContainerItemResult()
    {
    }

    public record Ok(GitHubArtifactItemJsonContent ArtifactItemContent)
        : DownloadContainerItemResult;

    public abstract record Error()
        : DownloadContainerItemResult;

    public record FailedToDownloadContainerItem(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : Error;

    public record ContainerItemContentNotJson(GitHubArtifactItemNotJsonContent NotJsonContent)
        : Error;

    public static implicit operator DownloadContainerItemResult(GitHubArtifactItemJsonContent artifactItemContent) => new Ok(artifactItemContent);

    public bool IsOk(
        [NotNullWhen(returnValue: true)] out GitHubArtifactItemJsonContent? artifactItemContent,
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
