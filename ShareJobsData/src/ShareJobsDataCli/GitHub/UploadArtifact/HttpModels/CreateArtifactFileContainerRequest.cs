namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

internal record CreateArtifactFileContainerRequest
{
    public CreateArtifactFileContainerRequest(string name)
    {
        Name = name.NotNullOrWhiteSpace();
    }

    public string Name { get; }

    public string Type => "actions_storage";
}
