namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels.Responses;

public record GitHubGetContainerItemsResponse
{
    public int Count { get; init; }

    [JsonPropertyName("value")]
    public List<GitHubContainerItem> ContainerItems { get; init; } = new List<GitHubContainerItem>();
}

public record GitHubContainerItem
{
    private string _contentLocation = default!;
    private string _path = default!;
    private string _itemType = default!;

    public int ContainerId { get; init; }

    public string ScopeIdentifier { get; init; } = default!;

    public string Path
    {
        get => _path;
        init => _path = value.NotNullOrWhiteSpace<GitHubContainerItem>(nameof(Path));
    }

    public string ItemType
    {
        get => _itemType;
        init => _itemType = value.NotNullOrWhiteSpace<GitHubContainerItem>(nameof(Path));
    }

    public string Status { get; init; } = default!;

    public DateTime DateCreated { get; init; }

    public DateTime DateLastModified { get; init; }

    public string CreatedBy { get; init; } = default!;

    public string LastModifiedBy { get; init; } = default!;

    public string ItemLocation { get; init; } = default!;

    public string ContentLocation
    {
        get => _contentLocation;
        init => _contentLocation = value.ValidUri<GitHubContainerItem>(nameof(ContentLocation));
    }

    public string ContentId { get; init; } = default!;

    public int FileLength { get; init; }

    public int FileEncoding { get; init; }

    public int FileType { get; init; }

    public int FileI { get; init; }
}
