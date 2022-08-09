namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.HttpModels.Responses;

internal record GitHubArtifactContainerItems
{
    public int Count { get; init; }

    [JsonPropertyName("value")]
    public IReadOnlyList<GitHubArtifactContainerItem> ContainerItems { get; init; } = new List<GitHubArtifactContainerItem>();
}
