namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.HttpModels;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.
internal record GitHubArtifactContainer(
    long ContainerId,
    long Size,
    string FileContainerResourceUrl,
    string Type,
    string Name,
    string Url,
    DateTime ExpiresOn);

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.

internal sealed class GitHubArtifactContainerValidator : AbstractValidator<GitHubArtifactContainer>
{
    public GitHubArtifactContainerValidator()
    {
        RuleFor(x => x.FileContainerResourceUrl)
            .Must(fileContainerResourceUrl => Uri.TryCreate(fileContainerResourceUrl, default(UriCreationOptions), out var _))
            .WithMessage(x => $"$.fileContainerResourceUrl is not a valid URL. Actual value: '{x.FileContainerResourceUrl}'.");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_ => "$.name must have a value.");
    }

    public GitHubArtifactContainerValidator(string collectionPath)
    {
        RuleFor(x => x.FileContainerResourceUrl)
            .Must(fileContainerResourceUrl => Uri.TryCreate(fileContainerResourceUrl, default(UriCreationOptions), out var _))
            .WithMessage(x => $"{collectionPath}[{{CollectionIndex}}].fileContainerResourceUrl is not a valid URL. Actual value: '{x.FileContainerResourceUrl}'.");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_ => $"{collectionPath}[{{CollectionIndex}}].name must have a value.");
    }
}
