using static ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact.Results.UploadArtifactFileResult;

namespace ShareJobsDataCli.CliCommands.Commands.SetData.Errors;

internal static class UploadArtifactFileResultErrorExtensions
{
    public static Task WriteToConsoleAsync(this Error uploadArtifactError, IConsole console, string command)
    {
        var error = uploadArtifactError switch
        {
            FailedToCreateArtifactContainer failedToCreateArtifactContainer => failedToCreateArtifactContainer.JsonHttpError.AsErrorMessage("upload artifact", "create an artifact container"),
            FailedToUploadArtifact failedToUploadArtifact => failedToUploadArtifact.JsonHttpError.AsErrorMessage("upload artifact", "upload an artifact container"),
            FailedToFinalizeArtifactContainer failedToFinalizeArtifactContainer => failedToFinalizeArtifactContainer.JsonHttpError.AsErrorMessage("upload artifact", "finalize artifact container"),
            _ => throw UnexpectedTypeException.Create(uploadArtifactError),
        };
        return console.WriteErrorAsync(command, error);
    }
}
