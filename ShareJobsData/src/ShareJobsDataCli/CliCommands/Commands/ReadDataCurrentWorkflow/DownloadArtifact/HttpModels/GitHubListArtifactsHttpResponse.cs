namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow.DownloadArtifact.HttpModels;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.
internal sealed record GitHubListArtifactsHttpResponse(
    int Count,
    [property: JsonPropertyName("value")] IReadOnlyList<GitHubArtifactContainer> Containers);

internal sealed record GitHubArtifactContainer(
    long ContainerId,
    long Size,
    string FileContainerResourceUrl,
    string Type,
    string Name,
    string Url,
    DateTime ExpiresOn);
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.

internal sealed class GitHubListArtifactsHttpResponseValidator : AbstractValidator<GitHubListArtifactsHttpResponse>
{
    public GitHubListArtifactsHttpResponseValidator()
    {
        RuleFor(x => x.Containers)
            .Must(x => x is not null)
            .WithMessage(_ => "$.value is missing from JSON response.");
        RuleForEach(x => x.Containers)
            .SetValidator(new GitHubArtifactContainerValidator("$.value"));
    }
}

internal sealed class GitHubArtifactContainerValidator : AbstractValidator<GitHubArtifactContainer>
{
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
