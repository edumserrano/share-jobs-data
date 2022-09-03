using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.UploadArtifactFile.Results.UploadArtifactFileResult;

namespace ShareJobsDataCli.CliCommands.Commands.SetData;

internal static class CommandExceptionExtensions
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static CommandException ToCommandException(this Error error)
    {
        error.NotNull();

        var details = error switch
        {
            FailedToCreateArtifactContainer failedToCreateArtifactContainer => failedToCreateArtifactContainer.ErrorResult.GetErrorDetails("creating an artifact container"),
            FailedToUploadArtifact failedToUploadArtifact => failedToUploadArtifact.ErrorResult.GetErrorDetails("uploading an artifact"),
            FailedToFinalizeArtifactContainer failedToFinalizeArtifactContainer => failedToFinalizeArtifactContainer.ErrorResult.GetErrorDetails("finalizing artifact container"),
            _ => throw UnexpectedTypeException.Create(error),
        };
        var exceptionMessage = new SetDataCommandExceptionMessage(details);
        return exceptionMessage.ToCommandException();
    }
}
