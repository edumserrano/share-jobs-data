using static ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact.Results.UploadArtifactFileResult;

namespace ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact.Results;

internal sealed class UploadArtifactFileResult
    : OneOfBase<GitHubFinalizeArtifactContainerHttpResponse,
        FailedToCreateArtifactContainer,
        FailedToUploadArtifact,
        FailedToFinalizeArtifactContainer>
{
    public UploadArtifactFileResult(
        OneOf<GitHubFinalizeArtifactContainerHttpResponse,
            FailedToCreateArtifactContainer,
            FailedToUploadArtifact,
            FailedToFinalizeArtifactContainer> _)
        : base(_)
    {
    }

    public sealed record FailedToCreateArtifactContainer(JsonHttpResult<GitHubCreateArtifactContainerHttpResponse>.Error JsonHttpError);

    public sealed record FailedToUploadArtifact(JsonHttpResult<GitHubUploadArtifactFileHttpResponse>.Error JsonHttpError);

    public sealed record FailedToFinalizeArtifactContainer(JsonHttpResult<GitHubFinalizeArtifactContainerHttpResponse>.Error JsonHttpError);

    public static implicit operator UploadArtifactFileResult(GitHubFinalizeArtifactContainerHttpResponse _) => new UploadArtifactFileResult(_);
    public static implicit operator UploadArtifactFileResult(FailedToCreateArtifactContainer _) => new UploadArtifactFileResult(_);
    public static implicit operator UploadArtifactFileResult(FailedToUploadArtifact _) => new UploadArtifactFileResult(_);
    public static implicit operator UploadArtifactFileResult(FailedToFinalizeArtifactContainer _) => new UploadArtifactFileResult(_);
}
