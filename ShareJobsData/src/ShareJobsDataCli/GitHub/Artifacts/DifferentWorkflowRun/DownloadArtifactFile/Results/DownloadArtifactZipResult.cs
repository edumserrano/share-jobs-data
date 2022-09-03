namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadArtifactZipResult
{
    private DownloadArtifactZipResult()
    {
    }

    public record Ok(ZipArchive ZipArchive)
        : DownloadArtifactZipResult;

    public record Error()
        : DownloadArtifactZipResult;

    public record FailedToDownloadArtifactZip(EnsureSuccessStatusCodeResult.Error ErrorResult)
        : Error;

    public static implicit operator DownloadArtifactZipResult(ZipArchive zipArchive) => new Ok(zipArchive);

    public bool IsOk(
        [NotNullWhen(returnValue: true)] out ZipArchive? zipArchive,
        [NotNullWhen(returnValue: false)] out Error? error)
    {
        zipArchive = null;
        error = null;

        if (this is Ok ok)
        {
            zipArchive = ok.ZipArchive;
            return true;
        }

        error = (Error)this;
        return false;
    }
}
