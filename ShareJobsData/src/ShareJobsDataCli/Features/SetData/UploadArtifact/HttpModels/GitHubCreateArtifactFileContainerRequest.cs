namespace ShareJobsDataCli.Features.SetData.UploadArtifact.HttpModels;

internal sealed record GitHubCreateArtifactContainerRequest
{
    public GitHubCreateArtifactContainerRequest(string name)
    {
        Name = name.NotNullOrWhiteSpace();
        Type = "actions_storage";
    }

    public string Name { get; }

    public string Type { get; }
}
