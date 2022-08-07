namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels.Responses;

public record GitHubGetContainerItemsResponse
{
    [JsonPropertyName("count")]
    public int Count { get; init; }

    [JsonPropertyName("value")]
    public List<GitHubContainerItem> Items { get; init; } = new List<GitHubContainerItem>();
}

public record GitHubContainerItem
{
    private string _contentId = default!;

    public int ContainerId { get; init; }

    public string ScopeIdentifier { get; init; } = default!;

    public string Path { get; init; } = default!;

    public string ItemType { get; init; } = default!;

    public string Status { get; init; } = default!;

    public DateTime DateCreated { get; init; }

    public DateTime DateLastModified { get; init; }

    public string CreatedBy { get; init; } = default!;

    public string LastModifiedBy { get; init; } = default!;

    public string ItemLocation { get; init; } = default!;

    public string ContentLocation { get; init; } = default!;

    public string ContentId
    {
        get => _contentId;
        init => _contentId = value.PropertyNotNullOrWhiteSpace<GitHubContainerItem>(nameof(ContentId));
    }

    public int FileLength { get; init; }

    public int FileEncoding { get; init; }

    public int FileType { get; init; }

    public int FileI { get; init; }
}

internal sealed class GitHubGetContainerItemsResponseValidator : AbstractValidator<GitHubGetContainerItemsResponse>
{
    public GitHubGetContainerItemsResponseValidator()
    {
        RuleForEach(x => x.Items)
            .SetValidator(new GitHubContainerItemValidator());
    }
}

internal sealed class GitHubContainerItemValidator : AbstractValidator<GitHubContainerItem>
{
    public GitHubContainerItemValidator()
    {
        RuleFor(x => x.ContentLocation)
            .Must(BeAValidUrl)
            .WithMessage(x => $"item {{CollectionIndex}}: {nameof(x.ContentLocation)} is not a valid URL. Actual value: '{x.ContentLocation}'.");
        RuleFor(x => x.ItemType)
            .NotEmpty()
            .WithMessage(x => $"item {{CollectionIndex}}: {nameof(x.ItemType)} must have a value.");
        RuleFor(x => x.Path)
            .NotEmpty()
            .WithMessage(x => $"item {{CollectionIndex}}: {nameof(x.Path)} must have a value.");
    }

    private bool BeAValidUrl(string fileContainerResourceUrl)
    {
        var options = new UriCreationOptions();
        return Uri.TryCreate(fileContainerResourceUrl, options, out var _);
    }
}
