namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

public record UploadArtifactFileResponse
(
    int containerId,
    string ScopeIdentifier,
    string Path,
    string ItemType,
    string Status,
    int FileLength,
    int FileEncoding,
    int FileType,
    DateTime DateCreated,
    DateTime DateLastModified,
    string CreatedBy,
    string LastModifiedBy,
    int FileId,
    string ContentId
);
