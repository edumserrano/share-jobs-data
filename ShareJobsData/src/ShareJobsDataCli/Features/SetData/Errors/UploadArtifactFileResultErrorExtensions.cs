using static ShareJobsDataCli.Features.SetData.UploadArtifact.Results.UploadArtifactFileResult;

namespace ShareJobsDataCli.Features.SetData.Errors;

internal static class UploadArtifactFileResultErrorExtensions
{
    [DoesNotReturn]
    public static void Throw(
        this OneOf<FailedToCreateArtifactContainer,
                    FailedToUploadArtifact,
                    FailedToFinalizeArtifactContainer> uploadArtifactError,
        string command)
    {
        uploadArtifactError.NotNull();
        var error = uploadArtifactError.Match(
            failedToCreateArtifactContainer => failedToCreateArtifactContainer.JsonHttpError.AsErrorMessage("upload artifact", "create an artifact container"),
            failedToUploadArtifact => failedToUploadArtifact.JsonHttpError.AsErrorMessage("upload artifact", "upload an artifact container"),
            failedToFinalizeArtifactContainer => failedToFinalizeArtifactContainer.JsonHttpError.AsErrorMessage("upload artifact", "finalize artifact container"));
        CommandExceptionThrowHelper.Throw(command, error);
    }
}
