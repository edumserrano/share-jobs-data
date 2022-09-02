namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.HttpModels;

internal record GitHubArtifactContainer
(
    long ContainerId,
    long Size,
    string FileContainerResourceUrl,
    string Type,
    string Name,
    string Url,
    DateTime ExpiresOn
);

internal sealed class GitHubArtifactContainerValidator : AbstractValidator<GitHubArtifactContainer>
{
    public GitHubArtifactContainerValidator()
    {
        RuleFor(x => x.FileContainerResourceUrl)
            .Must(fileContainerResourceUrl => Uri.TryCreate(fileContainerResourceUrl, default(UriCreationOptions), out var _))
            .WithMessage(x => $"{nameof(x.FileContainerResourceUrl)} is not a valid URL. Actual value: '{x.FileContainerResourceUrl}'.");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(x => $"{nameof(x.Name)} must have a value.");
    }

    public GitHubArtifactContainerValidator(string collectionPath)
    {
        RuleFor(x => x.FileContainerResourceUrl)
            .Must(fileContainerResourceUrl => Uri.TryCreate(fileContainerResourceUrl, default(UriCreationOptions), out var _))
            .WithMessage(x => $"{collectionPath}[{{CollectionIndex}}].{nameof(x.FileContainerResourceUrl)} is not a valid URL. Actual value: '{x.FileContainerResourceUrl}'.");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(x => $"{collectionPath}[{{CollectionIndex}}].{nameof(x.Name)} must have a value.");
    }
}