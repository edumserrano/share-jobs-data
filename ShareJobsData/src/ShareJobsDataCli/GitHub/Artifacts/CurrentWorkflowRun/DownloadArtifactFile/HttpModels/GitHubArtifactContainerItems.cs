namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.HttpModels;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.
internal record GitHubArtifactContainerItems(
    int Count,
    [property: JsonPropertyName("value")] IReadOnlyList<GitHubArtifactContainerItem> ContainerItems);
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.

internal sealed class GitHubArtifactContainerItemsValidator : AbstractValidator<GitHubArtifactContainerItems>
{
    public GitHubArtifactContainerItemsValidator()
    {
        RuleFor(x => x.ContainerItems)
            .Must(x => x is not null)
            .WithMessage(_ => "$.value is missing from JSON response.");
        RuleForEach(x => x.ContainerItems)
            .SetValidator(new GitHubArtifactContainerItemValidator("$.value"));
    }
}
