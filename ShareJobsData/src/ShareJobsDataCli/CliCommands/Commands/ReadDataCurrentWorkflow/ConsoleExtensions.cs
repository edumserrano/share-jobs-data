using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactFileFromCurrentWorkflowResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow;

internal static class ConsoleExtensions
{
    public static async Task WriteErrorAsync(this IConsole console, Error error)
    {
        console.NotNull();
        error.NotNull();

        var details = error switch
        {
            ArtifactNotFound artifactNotFound => $"Couldn't find artifact '{artifactNotFound.ArtifactContainerName}'.",
            ArtifactContainerItemNotFound artifactContainerItemNotFound => $"Couldn't find artifact file '{artifactContainerItemNotFound.ArtifactItemFilePath}'.",
            FailedToListWorkflowRunArtifacts failedToListWorkflowRunArtifacts => failedToListWorkflowRunArtifacts.JsonHttpError.ToErrorDetails("listing GitHub workflow artifacts"),
            FailedToGetContainerItems failedToGetContainerItems => failedToGetContainerItems.JsonHttpError.ToErrorDetails("retrieving GitHub workflow artifact container items"),
            FailedToDownloadArtifact failedToDownloadArtifact => GetErrorDetails(failedToDownloadArtifact),
            _ => throw UnexpectedTypeException.Create(error),
        };
        var errorMessage = new ReadDataFromCurrentWorkflowCommandErrorMessage(details);
        var msg = errorMessage.ToString();
        using var _ = console.WithForegroundColor(ConsoleColor.Red);
        await console.Error.WriteAsync(msg);
    }

    private static string GetErrorDetails(FailedToDownloadArtifact failedToDownloadArtifact)
    {
        var error = failedToDownloadArtifact.DownloadContainerItemError;
        return error switch
        {
            DownloadContainerItemResult.FailedToDownloadContainerItem failedToDownloadContainerItem => failedToDownloadContainerItem.FailedStatusCodeHttpResponse.ToErrorDetails("downloading GitHub artifact"),
            DownloadContainerItemResult.ContainerItemContentNotJson containerItemContentNotJson => containerItemContentNotJson.NotJsonContent.ToErrorDetails(),
            _ => throw UnexpectedTypeException.Create(error),
        };
    }
}
