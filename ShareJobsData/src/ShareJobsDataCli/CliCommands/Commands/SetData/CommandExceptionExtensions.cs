using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.UploadArtifactFile.Results.UploadArtifactFileResult;

namespace ShareJobsDataCli.CliCommands.Commands.SetData;

internal static class CommandExceptionExtensions
{
    public static CommandException ToCommandException(this Error error)
    {
        error.NotNull();

        var details = error switch
        {
            FailedToCreateArtifactContainer failedToCreateArtifactContainer => failedToCreateArtifactContainer.ErrorResult.ToErrorDetails("creating an artifact container"),
            FailedToUploadArtifact failedToUploadArtifact => failedToUploadArtifact.ErrorResult.ToErrorDetails("uploading an artifact"),
            FailedToFinalizeArtifactContainer failedToFinalizeArtifactContainer => failedToFinalizeArtifactContainer.ErrorResult.ToErrorDetails("finalizing artifact container"),
            _ => throw UnexpectedTypeException.Create(error),
        };
        var exceptionMessage = new SetDataCommandExceptionMessage(details);
        return exceptionMessage.ToCommandException();
    }
}
