using static ShareJobsDataCli.Features.SetData.UploadArtifact.Results.UploadArtifactFileResult;

namespace ShareJobsDataCli.Features.SetData.Errors;

internal static class UploadArtifactFileResultErrorExtensions
{
    public static Task WriteToConsoleAsync(this
        OneOf<FailedToCreateArtifactContainer,
            FailedToUploadArtifact,
            FailedToFinalizeArtifactContainer> uploadArtifactError,
        IConsole console,
        string command)
    {
        uploadArtifactError.NotNull();
        console.NotNull();
        command.NotNullOrWhiteSpace();

        var error = uploadArtifactError.Match(
            failedToCreateArtifactContainer => failedToCreateArtifactContainer.JsonHttpError.AsErrorMessage("upload artifact", "create an artifact container"),
            failedToUploadArtifact => failedToUploadArtifact.JsonHttpError.AsErrorMessage("upload artifact", "upload an artifact container"),
            failedToFinalizeArtifactContainer => failedToFinalizeArtifactContainer.JsonHttpError.AsErrorMessage("upload artifact", "finalize artifact container"));
        return console.WriteErrorAsync(command, error);
    }
}
