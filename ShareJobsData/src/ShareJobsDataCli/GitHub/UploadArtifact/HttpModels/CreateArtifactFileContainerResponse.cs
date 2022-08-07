namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

public record CreateArtifactFileContainerResponse
(
    int ContainerId,
    int Size,
    string FileContainerResourceUrl,
    string Type,
    string Name,
    string Url,
    DateTime ExpiresOn
);
