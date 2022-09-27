namespace ShareJobsDataCli.CliCommands.Commands.SetData.UploadArtifact.HttpModels;

internal sealed record GitHubCreateArtifactContainerRequest
{
    public GitHubCreateArtifactContainerRequest(string name)
    {
        Name = name.NotNullOrWhiteSpace();
    }

    public string Name { get; }

    public string Type { get; } = "actions_storage";
}
