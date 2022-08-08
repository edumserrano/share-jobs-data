namespace ShareJobsDataCli.GitHub.HttpModels.Responses;

internal sealed record GitHubArtifactFileContainerResponse
{
    private string _fileContainerResourceUrl = default!;
    private string _name = default!;

    public int ContainerId { get; init; }

    public int Size { get; init; }

    public string FileContainerResourceUrl
    {
        get => _fileContainerResourceUrl;
        init => _fileContainerResourceUrl = value.ValidUri<GitHubArtifactFileContainerResponse>(nameof(FileContainerResourceUrl));
    }

    public string Type { get; init; } = default!;

    public string Name
    {
        get => _name;
        init => _name = value.NotNullOrWhiteSpace<GitHubArtifactFileContainerResponse>(nameof(Name));
    }

    public string Url { get; init; } = default!;

    public DateTime ExpiresOn { get; init; }
}
