namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

public record UpdateArtifactFileContainerResponse
(
    int ContainerId,
    int Size,
    string FileContainerResourceUrl,
    string Type,
    string Name,
    string Url,
    DateTime ExpiresOn
);
