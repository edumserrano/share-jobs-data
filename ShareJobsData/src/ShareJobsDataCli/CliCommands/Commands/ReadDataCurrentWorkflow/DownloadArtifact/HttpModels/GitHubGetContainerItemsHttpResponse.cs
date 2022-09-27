namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact.HttpModels;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.
internal sealed record GitHubGetContainerItemsHttpResponse(
    int Count,
    [property: JsonPropertyName("value")] IReadOnlyList<GitHubArtifactContainerItem> ContainerItems);

internal sealed record GitHubArtifactContainerItem(
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

internal sealed class GitHubGetContainerItemsHttpResponseValidator : AbstractValidator<GitHubGetContainerItemsHttpResponse>
{
    public GitHubGetContainerItemsHttpResponseValidator()
    {
        RuleFor(x => x.ContainerItems)
            .Must(x => x is not null)
            .WithMessage(_ => "$.value is missing from JSON response.");
        RuleForEach(x => x.ContainerItems)
            .SetValidator(new GitHubArtifactContainerItemValidator("$.value"));
    }
}

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
