namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.HttpModels;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.
internal record GitHubArtifactContainers(
    int Count,
    [property: JsonPropertyName("value")] IReadOnlyList<GitHubArtifactContainer> Containers);
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.

internal sealed class GitHubArtifactContainersValidator : AbstractValidator<GitHubArtifactContainers>
{
    public GitHubArtifactContainersValidator()
    {
        RuleFor(x => x.Containers)
            .Must(x => x is not null)
            .WithMessage(x => $"'value' is missing from JSON response. {nameof(GitHubArtifactContainers)}.{nameof(x.Containers)} cannot be null.");
        RuleForEach(x => x.Containers)
            .SetValidator(new GitHubArtifactContainerValidator($"{nameof(GitHubArtifactContainers)}.{nameof(GitHubArtifactContainers.Containers)}"));
    }
}
