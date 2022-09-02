namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.HttpModels;

internal record GitHubArtifactContainers
(
    int Count,
    [property: JsonPropertyName("value")] IReadOnlyList<GitHubArtifactContainer> Containers
);

internal sealed class GitHubArtifactContainersValidator : AbstractValidator<GitHubArtifactContainers>
{
    public GitHubArtifactContainersValidator()
    {
        RuleFor(x => x.Containers)
            .Must(x => x is not null)
            .WithMessage(x => $"{nameof(x.Containers)} is missing from JSON response. {nameof(GitHubArtifactContainers)}.{nameof(x.Containers)} cannot be null.");
        RuleForEach(x => x.Containers)
            .SetValidator(new GitHubArtifactContainerValidator($"{nameof(GitHubArtifactContainers)}.{nameof(GitHubArtifactContainers.Containers)}"));
    }
}
