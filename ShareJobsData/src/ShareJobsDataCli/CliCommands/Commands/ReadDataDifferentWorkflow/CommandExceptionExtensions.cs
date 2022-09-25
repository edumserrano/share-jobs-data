using static ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactFileFromDifferentWorkflowResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow;

internal static class CommandExceptionExtensions
{
    public static CommandException ToCommandException(this Error error)
    {
        error.NotNull();

        var details = error switch
        {
            ArtifactNotFound artifactNotFound => $"Couldn't find artifact '{artifactNotFound.ArtifactContainerName}' in workflow run id '{artifactNotFound.WorkflowRunId}' at repo '{artifactNotFound.RepoName}'.",
            ArtifactFileNotFound artifactFileNotFound => $"Couldn't find artifact file '{artifactFileNotFound.ArtifactContainerName}/{artifactFileNotFound.ArtifactItemFilename}' in workflow run id '{artifactFileNotFound.WorkflowRunId}' at repo '{artifactFileNotFound.RepoName}'.",
            FailedToListWorkflowRunArtifacts failedToListWorkflowRunArtifacts => failedToListWorkflowRunArtifacts.JsonHttpError.ToErrorDetails("listing GitHub workflow artifacts"),
            FailedToDownloadArtifact failedToDownloadArtifact => failedToDownloadArtifact.FailedStatusCodeHttpResponse.ToErrorDetails("downloading GitHub artifact"),
            ArtifactItemContentNotJson artifactItemContentNotJson => artifactItemContentNotJson.NotJsonContent.ToErrorDetails(),
            _ => throw UnexpectedTypeException.Create(error),
        };
        var exceptionMessage = new ReadDataFromDifferentWorkflowCommandExceptionMessage(details);
        return exceptionMessage.ToCommandException();
    }
}
