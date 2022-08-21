namespace ShareJobsDataCli.GitHub.Artifacts.DifferentWorkflowRun.HttpModels.Responses;

internal record GitHubWorkflowRunArtifacts
(
    [property: JsonPropertyName("total_count")] int TotalCount,
    [property: JsonPropertyName("artifacts")] IReadOnlyList<GitHubWorkflowRunArtifact> Artifacts
);

internal record GitHubWorkflowRunArtifact
(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("node_id")] long NodeId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("size_in_bytes")] long SizeInBytes,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("archive_download_url")] string ArchiveDownloadUrl,
    [property: JsonPropertyName("expired")] bool Expired,
    [property: JsonPropertyName("created_at")] DateTime CreatedAt,
    [property: JsonPropertyName("expires_at")] DateTime ExpiresAt,
    [property: JsonPropertyName("updated_at")] DateTime UpdatedAt,
    [property: JsonPropertyName("workflow_run")] GitHubWorkflowRun WorkflowRun
);

internal record GitHubWorkflowRun
(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("repository_id")] long RepositoryId,
    [property: JsonPropertyName("head_repository_id")] long HeadRepositoryId,
    [property: JsonPropertyName("head_branch")] string HeadBranch,
    [property: JsonPropertyName("head_sha")] string HeadSha
);

internal sealed class WorkflowRunArtifactsValidator : AbstractValidator<GitHubWorkflowRunArtifacts>
{
    public WorkflowRunArtifactsValidator()
    {
        RuleFor(x => x.Artifacts)
            .Must(x => x is not null)
            .WithMessage(x => $"{nameof(x.Artifacts)} is missing from JSON response. {nameof(GitHubWorkflowRunArtifacts)}.{nameof(x.Artifacts)} cannot be null.");
        RuleForEach(x => x.Artifacts)
            .SetValidator(new GitHubWorkflowRunArtifactValidator($"{nameof(GitHubWorkflowRunArtifacts)}.{nameof(GitHubWorkflowRunArtifacts.Artifacts)}"));
    }
}

internal sealed class GitHubWorkflowRunArtifactValidator : AbstractValidator<GitHubWorkflowRunArtifact>
{
    public GitHubWorkflowRunArtifactValidator(string collectionPath)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(x => $"{collectionPath}[{{CollectionIndex}}].{nameof(x.Name)} must have a value.");
        RuleFor(x => x.ArchiveDownloadUrl)
            .Must(contentLocation => Uri.TryCreate(contentLocation, default(UriCreationOptions), out var _))
            .WithMessage(x => $"{collectionPath}[{{CollectionIndex}}].{nameof(x.ArchiveDownloadUrl)} is not a valid URL. Actual value: '{x.ArchiveDownloadUrl}'.");
    }
}
