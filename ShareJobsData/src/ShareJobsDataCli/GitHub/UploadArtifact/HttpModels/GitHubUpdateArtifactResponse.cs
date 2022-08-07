namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

internal sealed record GitHubUpdateArtifactResponse
(
    int ContainerId,
    string ScopeIdentifier,
    string Path,
    string ItemType,
    string Status,
    int FileLength,
    int FileEncoding,
    int FileType,
    DateTime DateCreated,
    DateTime DateLastModified,
    string CreatedBy,
    string LastModifiedBy,
    int FileId,
    string ContentId
);

internal sealed class GitHubUpdateArtifactResponseValidator : AbstractValidator<GitHubUpdateArtifactResponse>
{
    public GitHubUpdateArtifactResponseValidator()
    {
        RuleFor(x => x.FileLength)
            .Must(fileLength => fileLength > 0)
            .WithMessage(x => $"{nameof(x.FileLength)} is must be a positive value. Actual value: '{x.FileLength}'.");
    }
}
