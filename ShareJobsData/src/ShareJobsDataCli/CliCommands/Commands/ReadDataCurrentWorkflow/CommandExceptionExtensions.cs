using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactFileFromCurrentWorkflowResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow;

internal static class CommandExceptionExtensions
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static CommandException ToCommandException(this Error error)
    {
        error.NotNull();

        var details = error switch
        {
            ArtifactNotFound artifactNotFound => $"Couldn't find artifact '{artifactNotFound.ArtifactContainerName}'.",
            ArtifactContainerItemNotFound artifactContainerItemNotFound => $"Couldn't find artifact file '{artifactContainerItemNotFound.ArtifactItemFilePath}'.",
            FailedToListWorkflowRunArtifacts failedToListWorkflowRunArtifacts => failedToListWorkflowRunArtifacts.ErrorResult.GetErrorDetails("listing GitHub workflow artifacts"),
            FailedToGetContainerItems failedToGetContainerItems => failedToGetContainerItems.ErrorResult.GetErrorDetails("retrieving GitHub workflow artifact container items"),
            FailedToDownloadArtifact failedToDownloadArtifact => failedToDownloadArtifact.FailedStatusCodeHttpResponse.GetErrorDetails("downloading GitHub artifact"),
            _ => throw UnexpectedTypeException.Create(error),
        };
        var exceptionMessage = new ReadDataFromCurrentWorkflowCommandExceptionMessage(details);
        return exceptionMessage.ToCommandException();
    }
}
