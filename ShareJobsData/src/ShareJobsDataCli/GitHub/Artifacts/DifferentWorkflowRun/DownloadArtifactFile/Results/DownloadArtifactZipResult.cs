namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Results;

internal abstract record DownloadArtifactZipResult
{
    private DownloadArtifactZipResult()
    {
    }

    public record Ok(ZipArchive ZipArchive)
        : DownloadArtifactZipResult;

    public record FailedToDownloadArtifactZip : DownloadArtifactZipResult
    {
        public FailedToDownloadArtifactZip(EnsureSuccessStatusCodeResult errorResult)
        {
            if (errorResult is EnsureSuccessStatusCodeResult.Ok)
            {
                NotAnErrorResultException.Throw(errorResult);
            }

            ErrorResult = errorResult;
        }

        public EnsureSuccessStatusCodeResult ErrorResult { get; }
    }

    public static implicit operator DownloadArtifactZipResult(ZipArchive zipArchive) => new Ok(zipArchive);
}
