namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.HttpModels.Responses;

internal record GitHubArtifactContainerItems
(
    int Count,
    [property: JsonPropertyName("value")] IReadOnlyList<GitHubArtifactContainerItem> ContainerItems
);

internal sealed class GitHubArtifactContainerItemsValidator : AbstractValidator<GitHubArtifactContainerItems>
{
    public GitHubArtifactContainerItemsValidator()
    {
        RuleFor(x => x.ContainerItems)
            .Must(x => x is not null)
            .WithMessage(x => $"{nameof(x.ContainerItems)} is missing from JSON response. {nameof(GitHubArtifactContainerItems)}.{nameof(x.ContainerItems)} cannot be null.");
        RuleForEach(x => x.ContainerItems)
            .SetValidator(new GitHubArtifactContainerItemValidator($"{nameof(GitHubArtifactContainerItems)}.{nameof(GitHubArtifactContainerItems.ContainerItems)}"));
    }
}
