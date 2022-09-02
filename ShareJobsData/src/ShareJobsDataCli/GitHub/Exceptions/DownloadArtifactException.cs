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

    //internal static DownloadArtifactException ArtifactNotFound(string repoName, string workflowRunId, string artifactName)
    //{
    //    var message = $"Couldn't find artifact '{artifactName}' in workflow run id {workflowRunId} at repo {repoName}.";
    //    return new DownloadArtifactException(message);
    //}

    internal static DownloadArtifactException ArtifactFileNotFound(string artifactFilePath)
    {
        var message = $"Couldn't find artifact file '{artifactFilePath}' in the current workflow run.";
        return new DownloadArtifactException(message);
    }

    //internal static DownloadArtifactException ArtifactFileNotFound(string repoName, string workflowRunId, string artifactFilePath)
    //{
    //    var message = $"Couldn't find artifact file '{artifactFilePath}' in workflow run id {workflowRunId} at repo {repoName}.";
    //    return new DownloadArtifactException(message);
    //}
}
