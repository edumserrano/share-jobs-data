using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactFileFromCurrentWorkflowResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow;

internal static class CommandExceptionExtensions
{
    public static CommandException ToCommandException(this Error error)
    {
        error.NotNull();

        var details = error switch
        {
            ArtifactNotFound artifactNotFound => $"Couldn't find artifact '{artifactNotFound.ArtifactContainerName}'.",
            ArtifactContainerItemNotFound artifactContainerItemNotFound => $"Couldn't find artifact file '{artifactContainerItemNotFound.ArtifactItemFilePath}'.",
            FailedToListWorkflowRunArtifacts failedToListWorkflowRunArtifacts => failedToListWorkflowRunArtifacts.ErrorResult.ToErrorDetails("listing GitHub workflow artifacts"),
            FailedToGetContainerItems failedToGetContainerItems => failedToGetContainerItems.ErrorResult.ToErrorDetails("retrieving GitHub workflow artifact container items"),
            FailedToDownloadArtifact failedToDownloadArtifact => GetErrorDetails(failedToDownloadArtifact),
            _ => throw UnexpectedTypeException.Create(error),
        };
        var exceptionMessage = new ReadDataFromCurrentWorkflowCommandExceptionMessage(details);
        return exceptionMessage.ToCommandException();
    }

    private static string GetErrorDetails(FailedToDownloadArtifact failedToDownloadArtifact)
    {
        var error = failedToDownloadArtifact.DownloadContainerItemError;
        return error switch
        {
            DownloadContainerItemResult.ArtifactItemContentNotJson artifactItemContentNotJson => $"Artifact file content was not valid JSON. Got: '{artifactItemContentNotJson.ArtifactItemContent}'.",
            DownloadContainerItemResult.FailedToDownloadContainerItem failedToDownloadContainerItem => failedToDownloadContainerItem.FailedStatusCodeHttpResponse.ToErrorDetails("downloading GitHub artifact"),
            _ => throw UnexpectedTypeException.Create(error),
        };
    }
}
