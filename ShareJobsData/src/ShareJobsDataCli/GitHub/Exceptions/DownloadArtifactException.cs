namespace ShareJobsDataCli.GitHub.Exceptions;

public sealed class DownloadArtifactException : Exception
{
    internal DownloadArtifactException(string message)
        : base(message)
    {
    }

    internal static DownloadArtifactException ArtifactNotFound(string artifactName)
    {
        var message = $"Couldn't find artifact '{artifactName}' in the current workflow run.";
        return new DownloadArtifactException(message);
    }

    internal static DownloadArtifactException ArtifactFileNotFound(string artifactFilePath)
    {
        var message = $"Couldn't find artifact file '{artifactFilePath}' in the current workflow run.";
        return new DownloadArtifactException(message);
    }
}
