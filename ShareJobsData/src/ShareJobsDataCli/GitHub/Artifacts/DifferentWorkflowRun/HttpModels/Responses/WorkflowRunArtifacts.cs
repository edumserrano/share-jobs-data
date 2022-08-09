namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.HttpModels.Responses;

internal record WorkflowRunArtifacts
{
    [JsonPropertyName("total_count")]
    public int TotalCount { get; init; }

    [JsonPropertyName("artifacts")]
    public IReadOnlyList<WorkflowRunArtifact> Artifacts { get; init; } = new List<WorkflowRunArtifact>();
}

internal record WorkflowRunArtifact
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("node_id")]
    public string NodeId { get; init; } = default!;

    [JsonPropertyName("name")]
    public string Name { get; init; } = default!;

    [JsonPropertyName("size_in_bytes")]
    public long SizeInBytes { get; init; }

    [JsonPropertyName("url")]
    public string Url { get; init; } = default!;

    [JsonPropertyName("archive_download_url")]
    public string ArchiveDownloadUrl { get; init; } = default!;

    [JsonPropertyName("expired")]
    public bool Expired { get; init; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; init; }

    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; init; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; init; }

    [JsonPropertyName("workflow_run")]
    public WorkflowRun WorkflowRun { get; init; } = default!;
}

internal record WorkflowRun
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("repository_id")]
    public long RepositoryId { get; init; }

    [JsonPropertyName("head_repository_id")]
    public long HeadRepositoryId { get; init; }

    [JsonPropertyName("head_branch")]
    public string HeadBranch { get; init; } = default!;

    [JsonPropertyName("head_sha")]
    public string HeadSha { get; init; } = default!;
}
