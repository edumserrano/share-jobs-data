namespace ShareJobsDataCli.GitHub;

public class CreateArtifactFileContainerResponse
{
    public int ContainerId { get; set; }

    public int Size { get; set; }

    public string FileContainerResourceUrl { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Url { get; set; } = default!;

    public DateTime expiresOn { get; set; }
}
