namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadArtifactZipResult
{
    private DownloadArtifactZipResult()
    {
    }

    public record Ok(ZipArchive ZipArchive)
        : DownloadArtifactZipResult;

    public record FailedToDownloadArtifactZip(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : DownloadArtifactZipResult;

    public static implicit operator DownloadArtifactZipResult(ZipArchive zipArchive) => new Ok(zipArchive);

    public bool IsOk(
        [NotNullWhen(returnValue: true)] out ZipArchive? zipArchive,
        [NotNullWhen(returnValue: false)] out FailedStatusCodeHttpResponse? failedStatusCodeHttpResponse)
    {
        zipArchive = null;
        failedStatusCodeHttpResponse = null;

        if (this is Ok ok)
        {
            zipArchive = ok.ZipArchive;
            return true;
        }

        var failedToDownloadArtifactZip = (FailedToDownloadArtifactZip)this;
        failedStatusCodeHttpResponse = failedToDownloadArtifactZip.FailedStatusCodeHttpResponse;
        return false;
    }
}
