namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.HttpModels;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.
internal record GitHubArtifactContainerItem(
    long ContainerId,
    string ScopeIdentifier,
    string Path,
    string ItemType,
    string Status,
    DateTime DateCreated,
    DateTime DateLastModified,
    string CreatedBy,
    string LastModifiedBy,
    string ItemLocation,
    string ContentLocation,
    string ContentId,
    long FileLength,
    long FileEncoding,
    long FileType,
    long FileId);
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.

internal sealed class GitHubArtifactContainerItemValidator : AbstractValidator<GitHubArtifactContainerItem>
{
    public GitHubArtifactContainerItemValidator(string collectionPath)
    {
        RuleFor(x => x.ContentLocation)
            .Must(contentLocation => Uri.TryCreate(contentLocation, default(UriCreationOptions), out var _))
            .WithMessage(x => $"{collectionPath}[{{CollectionIndex}}].{nameof(x.ContentLocation)} is not a valid URL. Actual value: '{x.ContentLocation}'.");
        RuleFor(x => x.ItemType)
            .NotEmpty()
            .WithMessage(x => $"{collectionPath}[{{CollectionIndex}}].{nameof(x.ItemType)} must have a value.");
        RuleFor(x => x.Path)
            .NotEmpty()
            .WithMessage(x => $"{collectionPath}[{{CollectionIndex}}].{nameof(x.Path)} must have a value.");
    }
}
