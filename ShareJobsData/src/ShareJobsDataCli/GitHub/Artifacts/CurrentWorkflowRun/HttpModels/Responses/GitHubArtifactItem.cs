namespace ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.HttpModels.Responses;

internal record GitHubArtifactItem
{
    private int _fileLength;

    public int ContainerId { get; init; }

    public string ScopeIdentifier { get; init; } = default!;

    public string Path { get; init; } = default!;

    public string ItemType { get; init; } = default!;

    public string Status { get; init; } = default!;

    public int FileLength
    {
        get => _fileLength;
        init => _fileLength = value.Positive<GitHubArtifactItem>(nameof(FileLength));
    }

    public int FileEncoding { get; init; }

    public int FileType { get; init; }

    public DateTime DateCreated { get; init; }

    public DateTime DateLastModified { get; init; }

    public string CreatedBy { get; init; } = default!;

    public string LastModifiedBy { get; init; } = default!;

    public int FileId { get; init; }

    public string ContentId { get; init; } = default!;
}
