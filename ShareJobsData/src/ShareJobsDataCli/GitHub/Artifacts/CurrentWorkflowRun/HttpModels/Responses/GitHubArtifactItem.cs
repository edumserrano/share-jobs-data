namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.HttpModels.Responses;

internal record GitHubArtifactItem
(
    long ContainerId,
    string ScopeIdentifier,
    string Path,
    string ItemType,
    string Status,
    long FileLength,
    int FileEncoding,
    int FileType,
    DateTime DateCreated,
    DateTime DateLastModified,
    string CreatedBy,
    string LastModifiedBy,
    long FileId,
    string ContentId
);

internal sealed class GitHubArtifactItemValidator : AbstractValidator<GitHubArtifactItem>
{
    public GitHubArtifactItemValidator()
    {
        RuleFor(x => x.FileLength)
            .Must(fileLength => fileLength > 0)
            .WithMessage(x => $"{nameof(x.FileLength)} is must be a positive value. Actual value: '{x.FileLength}'.");
    }
}
