namespace ShareJobsDataCli.GitHub.HttpModels.Responses;

internal sealed record GitHubListArtifactsResponse
{
    public int Count { get; init; }

    [JsonPropertyName("value")]
    public IReadOnlyList<GitHubArtifactFileContainerResponse> ArtifactFileContainers { get; init; } = new List<GitHubArtifactFileContainerResponse>();
}
