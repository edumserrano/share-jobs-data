using static ShareJobsDataCli.Features.ReadDataCurrentWorkflow.DownloadArtifact.Results.DownloadArtifactFileFromCurrentWorkflowResult;

namespace ShareJobsDataCli.Features.ReadDataCurrentWorkflow.Errors;

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
            ArtifactContainerItemNotFound artifactContainerItemNotFound => GetErrorMessage(artifactContainerItemNotFound),
            FailedToListWorkflowRunArtifacts failedToListWorkflowRunArtifacts => failedToListWorkflowRunArtifacts.JsonHttpError.AsErrorMessage("download artifact", "list workflow artifacts"),
            FailedToGetContainerItems failedToGetContainerItems => failedToGetContainerItems.JsonHttpError.AsErrorMessage("download artifact", "retrieve workflow artifact container items"),
            FailedToDownloadArtifact failedToDownloadArtifact => GetErrorDetails(failedToDownloadArtifact),
            _ => throw UnexpectedTypeException.Create(downloadArtifactError),
        };
        return console.WriteErrorAsync(command, error);
    }

    private static string GetErrorDetails(FailedToDownloadArtifact failedToDownloadArtifact)
    {
        var error = failedToDownloadArtifact.DownloadContainerItemError;
        return error switch
        {
            DownloadContainerItemResult.FailedToDownloadContainerItem failedToDownloadContainerItem => $"Failed to download artifact because the HTTP request returned an error status code. {failedToDownloadContainerItem.FailedStatusCodeHttpResponse.AsHttpErrorMessage()}",
            DownloadContainerItemResult.ContainerItemContentNotJson containerItemContentNotJson => containerItemContentNotJson.NotJsonContent.AsErrorMessage(),
            _ => throw UnexpectedTypeException.Create(error),
        };
    }

    private static string GetErrorMessage(ArtifactNotFound artifactNotFound)
    {
        return $"Couldn't find artifact with name '{artifactNotFound.ArtifactContainerName}' in current workflow run.";
    }

    private static string GetErrorMessage(ArtifactContainerItemNotFound artifactContainerItemNotFound)
    {
        return $"Couldn't find artifact file '{artifactContainerItemNotFound.ArtifactItemFilePath}' in current workflow run.";
    }
}
