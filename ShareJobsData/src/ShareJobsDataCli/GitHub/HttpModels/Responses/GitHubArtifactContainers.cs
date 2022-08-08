namespace ShareJobsDataCli.GitHub.HttpModels.Responses;

internal sealed record GitHubArtifactContainers
{
    public int Count { get; init; }

    [JsonPropertyName("value")]
    public IReadOnlyList<GitHubArtifactContainer> Containers { get; init; } = new List<GitHubArtifactContainer>();
}
