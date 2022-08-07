namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

internal sealed record GitHubCreateArtifactFileContainerRequest
{
    public GitHubCreateArtifactFileContainerRequest(string name)
    {
        Name = name.NotNullOrWhiteSpace();
    }

    public string Name { get; }

    public string Type => "actions_storage";
}
