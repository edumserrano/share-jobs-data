namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.UploadArtifactFile.HttpModels;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.
internal record GitHubArtifactItem(
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
    string ContentId);
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.

internal sealed class GitHubArtifactItemValidator : AbstractValidator<GitHubArtifactItem>
{
    public GitHubArtifactItemValidator()
    {
        RuleFor(x => x.FileLength)
            .Must(fileLength => fileLength > 0)
            .WithMessage(x => $"$.fileLength must be a positive value. Actual value: '{x.FileLength}'.");
    }
}
