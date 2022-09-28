using static ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.DownloadArtifact.Results.DownloadArtifactFileFromDifferentWorkflowResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow.Errors;

internal static class DownloadArtifactErrorExtensions
{
    public static Task WriteToConsoleAsync(this Error downloadArtifactError, IConsole console, string command)
    {
        downloadArtifactError.NotNull();
        console.NotNull();
        command.NotNullOrWhiteSpace();

        var error = downloadArtifactError switch
        {
            ArtifactNotFound artifactNotFound => GetErrorMessage(artifactNotFound),
            ArtifactFileNotFound artifactFileNotFound => GetErrorMessage(artifactFileNotFound),
            ArtifactItemContentNotJson artifactItemContentNotJson => artifactItemContentNotJson.NotJsonContent.AsErrorMessage(),
            FailedToDownloadArtifact failedToDownloadArtifact => GetErrorMessage(failedToDownloadArtifact),
            FailedToListWorkflowRunArtifacts failedToListWorkflowRunArtifacts => failedToListWorkflowRunArtifacts.JsonHttpError.AsErrorMessage("download artifact", "list workflow artifacts"),
            _ => throw UnexpectedTypeException.Create(downloadArtifactError),
        };
        return console.WriteErrorAsync(command, error);
    }

    private static string GetErrorMessage(ArtifactNotFound artifactNotFound)
    {
        return $"Couldn't find artifact with name '{artifactNotFound.ArtifactContainerName}' in workflow run id '{artifactNotFound.WorkflowRunId}' at repo '{artifactNotFound.RepoName}'.";
    }

    private static string GetErrorMessage(ArtifactFileNotFound artifactFileNotFound)
    {
        return $"Couldn't find artifact file '{artifactFileNotFound.ArtifactContainerName}/{artifactFileNotFound.ArtifactItemFilename}' in workflow run id '{artifactFileNotFound.WorkflowRunId}' at repo '{artifactFileNotFound.RepoName}'.";
    }

    private static string GetErrorMessage(FailedToDownloadArtifact failedToDownloadArtifact)
    {
        var httpErrorMessage = failedToDownloadArtifact.FailedStatusCodeHttpResponse.AsHttpErrorMessage();
        return $"Failed to download artifact because the HTTP request returned an error status code. {httpErrorMessage}";
    }
}
